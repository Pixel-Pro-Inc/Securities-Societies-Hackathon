using API.DTO;
using API.Entities;
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
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageController( IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto, User user/*We need this from wherever its being called*/)
        {

            if (user.username == createMessageDto.RecipientUsername.ToLower()) return BadRequest("You cannot send messages to yourself");

            var sender = await GetUser(user.Email);//gets the sender
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

            _firebaseDataContext.StoreData("Messages/" + user.Id+ message.Id, message);
            //return firebase style expected

            //The lines below may give problems cause there is no database context in MessageRepository.
            //But I'm leaving them opening here in case we switch to sql for some God forsaken reason. In any case they don't hurt anyone
            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

            //if not made @46 put the return method with or without the mapper with MessageDto

            return BadRequest("Failed to send Message");
        }

    }
}
