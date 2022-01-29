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
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto, string user)
        {
            var username = user;
            if (username == createMessageDto.RecipientUsername.ToLower()) return BadRequest("You cannot send messages to yourself");

            var sender = await GetUser(user);
            var recipient = await GetUser(createMessageDto.RecipientUsername);
            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.username,
                RecipientUsername = recipient.username,
                content = createMessageDto.Content
            };
            //The two lines below may give problems cause there is no database context in MessageRepository. Consider replacing somehow with firebase there.
            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send Message");
        }

    }
}
