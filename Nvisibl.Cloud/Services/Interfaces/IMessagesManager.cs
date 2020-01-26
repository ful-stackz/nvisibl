using Nvisibl.Cloud.Models.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Services.Interfaces
{
    public interface IMessagesManager
    {
        Task<MessageModel> CreateMessageAsync(CreateMessageModel messageModel);

        Task<IEnumerable<MessageModel>> GetChatroomMessagesAsync(int id, int page, int pageSize);

        Task<IEnumerable<MessageModel>> GetUserMessagesAsync(int id, int page, int pageSize);
    }
}