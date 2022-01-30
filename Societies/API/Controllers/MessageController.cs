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

        #region Create Messages
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

            _firebaseDataContext.StoreData("Messages/" + sender.Id+ message.Id, message); //Consider replacing message with _mapper.Map<MessageDto>(message)
            //return firebase style expected

            //The lines below may give problems cause there is no database context in MessageRepository.
            /*
            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));
             */
            return Ok(_mapper.Map<MessageDto>(message));

        }
        #endregion
        #region Get Messages 
        //apparently we don't add anything here cause we will be using query string
        [HttpGet]
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
        #endregion
        #region Get Message Thread
        [HttpGet("thread/{otherusername}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagedThread(string UserEmail, string otherusername)
        {
            User sender = await GetUser(UserEmail); //We using this method cause this is all Yewo left to our disposal and I'm not going to write more code
            var currentUsername = sender.username;

            return Ok(await GetThread(currentUsername,otherusername));

        }

        async Task<IEnumerable<MessageDto>> GetThread(string currentUsername, string recipientUsername)
        {
            var answer= await _firebaseDataContext.GetData<Message>("Messages");

            //This gets all the messages that involve the user and had sent between them and some other person. Basically their conversations
            var messages = answer.Where(m=>m.Recipient.username==currentUsername
                &&m.Sender.username==m.Recipient.username
                ||m.Recipient.username==recipientUsername
                &&m.Sender.username==currentUsername)
                .OrderBy(m=>m.MessageSent)
                .ToList();

            //Checks for unread messages
            var unreadMessages= messages.Where(m=> m.DateRead==null&&m.Recipient.username==currentUsername).ToList(); 
            if(unreadMessages.Any())
            {
                foreach(var message in unreadMessages)
                {
                    message.DateRead=DateTime.Now;
                    _firebaseDataContext.EditData("Messages/"+message.SenderId+message.Id,message);
                    /*I want it to overide just the message DateRead so We might need to introduce a new node here  */ 
                }
            }
            return _mapper.Map<IEnumerable<MessageDto>>(messages);

        }

        #endregion
        

    }
}
