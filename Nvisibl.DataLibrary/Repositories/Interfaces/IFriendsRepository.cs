using Nvisibl.DataLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nvisibl.DataLibrary.Repositories.Interfaces
{
    public interface IFriendsRepository : IRepositoryT<Friend>
    {
        Task<IEnumerable<User>> GetAllFriendsAsync(User user);
    }
}
