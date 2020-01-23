using Nvisibl.Cloud.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Services.Interfaces
{
    public interface IChatroomManagerService
    {
        Task<ChatroomModel> CreateChatroomAsync(CreateChatroomModel createChatroomModel);
        Task<IEnumerable<ChatroomModel>> GetAllUserChatroomsAsync(UserModel user);
        Task<ChatroomModel> GetChatroomByIdAsync(int id);
        Task<ChatroomWithUsersModel> GetChatroomByIdWithUsersAsync(int id);
        Task<IEnumerable<ChatroomModel>> GetChatroomsAsync(int page = 0, int pageSize = 10);
        Task DeleteChatroomAsync(int id);
        Task AddUserToChatroomAsync(int chatroomId, UserModel user);
    }
}