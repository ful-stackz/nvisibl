using Nvisibl.DataLibrary.Contexts;
using Nvisibl.DataLibrary.Models;
using Nvisibl.DataLibrary.Repositories.Interfaces;

namespace Nvisibl.DataLibrary.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ChatContext dbContext)
            : base(dbContext)
        {
        }
    }
}
