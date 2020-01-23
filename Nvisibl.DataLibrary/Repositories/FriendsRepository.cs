using Nvisibl.DataLibrary.Contexts;
using Nvisibl.DataLibrary.Models;
using Nvisibl.DataLibrary.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nvisibl.DataLibrary.Repositories
{
    [Repository]
    internal class FriendsRepository : Repository<Friend>, IFriendsRepository
    {
        public FriendsRepository(ChatContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetAllFriendsAsync(User user)
        {
            return await Context.Set<Friend>()
                .Where(f => f.User1Id == user.Id || f.User2Id == user.Id)
                .Include(f => f.User1)
                .Include(f => f.User2)
                .Select(f => user.Id == f.User1Id ? f.User2 : f.User1)
                .ToListAsync();
        }
    }
}
