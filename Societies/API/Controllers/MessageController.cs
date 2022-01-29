using API.DTO;
using API.Entities;
using API.Extentions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class MessageController:BaseApiController
    {
        //private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageController( /*IMessageRepository messageRepository,*/ IMapper mapper)
        {
           // _messageRepository = messageRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            User sender = await GetUser(createMessageDto.SenderEmail);
            if (createMessageDto.SenderEmail == createMessageDto.RecipientUsername.ToLower()) return BadRequest("You cannot send messages to yourself");

            var recipient = await GetUser(createMessageDto.RecipientEmail);
            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.username,
                RecipientUsername = recipient.username,
                content = createMessageDto.Content
            };

            _firebaseDataContext.StoreData("Messages/" + sender.Id+ message.Id, message);
            //return firebase style expected

            //The lines below may give problems cause there is no database context in MessageRepository.
            /*
            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));
             */
            return Ok(_mapper.Map<MessageDto>(message));

        }

        
        [HttpGet]//apparently we don't add anything here cause we will be using query string
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            var messages = await GetMessages(messageParams); //firebase call
            Response.AddPaginationHeader(messages.CurrentPage, messages.Pagesize, messages.TotalCount, messages.TotalPages);
            return messages;
        }

        async Task<PagedList<MessageDto>> GetMessages(MessageParams messageParams)
        {
            ///Use this as a template for the firebase equivalent in MessageController
            var query = await _firebaseDataContext.GetData<Message>("Messages");
            IQueryable<Message> checlist= query.AsQueryable<Message>(); //needs to be IQueryable 
            checlist = messageParams.Container switch
            {
                "Inbox" => checlist.Where(u => u.Recipient.username == messageParams.Username),
                "Outbox" => checlist.Where(u => u.Sender.username == messageParams.Username),
                _ => checlist.Where(u => u.Recipient.username == messageParams.Username && u.DateRead == null)
            };

            var messages = checlist.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.pageSize);
        }
    }
}
