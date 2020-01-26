using Nvisibl.Business.Models.Chatrooms;
using Nvisibl.Business.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nvisibl.Business.Interfaces
{
    public interface IChatroomsManager
    {
        Task<ChatroomModel> CreateChatroomAsync(CreateChatroomModel createChatroomModel);

        Task<ChatroomModel> GetChatroomAsync(int id);

        Task<IEnumerable<ChatroomModel>> GetChatroomsAsync(int page, int pageSize);

        Task DeleteChatroomAsync(int id);

        Task AddUserToChatroomAsync(AddUserToChatroomModel addUserToChatroomModel);

        Task<IEnumerable<ChatroomModel>> GetUserChatroomsAsync(UserModel userModel);

        Task<IEnumerable<UserModel>> GetChatroomUsersAsync(int id);
    }
}