using Nvisibl.DataLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nvisibl.DataLibrary.Repositories.Interfaces
{
    public interface IChatroomRepository : IRepository<Chatroom>
    {
        Task<IEnumerable<Chatroom>> GetAllUserChatroomsAsync(User user);
        Task<IEnumerable<User>> GetAllChatroomUsers(int id);
    }
}
