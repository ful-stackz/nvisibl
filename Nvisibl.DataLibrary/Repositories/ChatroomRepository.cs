using Microsoft.EntityFrameworkCore;
using Nvisibl.DataLibrary.Contexts;
using Nvisibl.DataLibrary.Models;
using Nvisibl.DataLibrary.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nvisibl.DataLibrary.Repositories
{
    [Repository]
    internal class ChatroomRepository : Repository<Chatroom>, IChatroomRepository
    {
        public ChatroomRepository(ChatContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Chatroom>> GetAllUserChatroomsAsync(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await Context.Set<ChatroomUser>()
                .Where(cu => cu.UserId == user.Id)
                .Include(cu => cu.Chatroom)
                .Select(cu => cu.Chatroom)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllChatroomUsers(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            return await Context.Set<ChatroomUser>()
                .Where(cu => cu.ChatroomId == id)
                .Include(c => c.User)
                .Select(c => c.User)
                .ToListAsync();
        }
    }
}
