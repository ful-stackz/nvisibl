using Microsoft.EntityFrameworkCore;
using Nvisibl.DataLibrary.Models;
using Nvisibl.DataLibrary.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nvisibl.DataLibrary.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(DbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Message>> GetChatroomMessagesAsync(int id, int page = 0, int pageSize = 20)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            return await Context.Set<Message>()
                .Where(m => m.ChatroomId == id)
                .Include(m => m.Author)
                .Skip((page <= 0 ? 0 : pageSize - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetUserMessagesAsync(int id, int page = 0, int pageSize = 20)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            return await Context.Set<Message>()
                .Where(m => m.AuthorId == id)
                .Include(m => m.Chatroom)
                .Skip((page <= 0 ? 0 : pageSize - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
