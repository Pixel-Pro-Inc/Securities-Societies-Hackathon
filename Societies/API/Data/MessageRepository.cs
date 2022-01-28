using API.DTO;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        // Consider Completely changing all of this cause from here we need to go to extentions (which doesn't exist) and go to
        /** AddApplicationsServices() and add this line services.AddScoped<IMessageRepository, MessageRepository>();
         * 
         */
        private readonly DataContext _context;

        public MessageRepository(DataContext context)
        {
            _context = context;
        }

        public void AddMessage(Message message) => _context.Messages.Add(message);

        public void DeleteMessage(Message message) => _context.Messages.Remove(message);

        public async Task<Message> GetMessage(int id) => await _context.Messages.FindAsync(id);

        public Task<PagedList<MessageDto>> GetMessagesForUser()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync() => await _context.SaveChangesAsync() > 0;
    }
}
