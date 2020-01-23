using Nvisibl.Cloud.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Services.Interfaces
{
    public interface IMessagesManagerService
    {
        Task<MessageModel> CreateMessageAsync(CreateMessageModel createMessageModel);
        Task<IEnumerable<MessageWithAuthorModel>> GetChatroomMessagesAsync(int chatroomId, int page, int pageSize);
        Task<IEnumerable<MessageWithChatroomModel>> GetUserMessagesAsync(int userId, int page, int pageSize);
    }
}