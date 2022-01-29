﻿using API.DTO;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        /* Consider Completely changing all of this cause I seems this is only useful for database items
         * Or at least make any of these silent/obselete

         */
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddMessage(Message message) => _context.Messages.Add(message);

        public void DeleteMessage(Message message) => _context.Messages.Remove(message);

        public async Task<Message> GetMessage(int id) => await _context.Messages.FindAsync(id);

        [Obsolete] //Don't use this method. It neeeeeds datacontext
        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            ///Use this as a template for the firebase equivalent in MessageController
            var query = _context.Messages
                .OrderByDescending(m => m.MessageSent)
                .AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Recipient.username == messageParams.Username),
                "Outbox" => query.Where(u => u.Sender.username == messageParams.Username),
                _ => query.Where(u => u.Recipient.username == messageParams.Username && u.DateRead == null)
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.pageSize);
        }

        public Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync() => await _context.SaveChangesAsync() > 0;
    }
}
