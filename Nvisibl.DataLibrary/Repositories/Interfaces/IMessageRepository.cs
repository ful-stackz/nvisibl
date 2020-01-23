using Nvisibl.DataLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nvisibl.DataLibrary.Repositories.Interfaces
{
    public interface IMessageRepository : IRepositoryT<Message>
    {
        Task<IEnumerable<Message>> GetChatroomMessagesAsync(int id, int page = 0, int pageSize = 20);
        Task<IEnumerable<Message>> GetUserMessagesAsync(int id, int page = 0, int pageSize = 20);
    }
}
