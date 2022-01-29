using API.DTO;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
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
        User SenderUser;
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {

            if (SenderUser.username == createMessageDto.RecipientUsername.ToLower()) return BadRequest("You cannot send messages to yourself");

            var sender = await GetUser(SenderUser.Email);//gets the sender. We need to do better. I'll pull from yewo and see if he already has something similar
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

            _firebaseDataContext.StoreData("Messages/" + SenderUser.Id+ message.Id, message);
            //return firebase style expected

            //The lines below may give problems cause there is no database context in MessageRepository.
            /*
            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));
             */
            return Ok(_mapper.Map<MessageDto>(message));

        }

        /*
          [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUser// gets the sender. We need to do better.I'll pull from yewo and see if he already has something similar
        }

         */
       
    }
}
