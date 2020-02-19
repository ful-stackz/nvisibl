using Nvisibl.Business.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nvisibl.Business.Interfaces
{
    public interface IUsersManager
    {
        Task<UserModel> CreateUserAsync(CreateUserModel createUserModel);

        Task<FriendModel> CreateFriendRequestAsync(AddUserFriendModel addUserFriendModel);

        Task<UserModel> GetUserAsync(int id);

        Task<UserModel?> GetUserAsync(string username);

        Task<FriendModel> GetFriendRequestAsync(int id);

        Task<IEnumerable<UserModel>> GetUsersAsync(int page, int pageSize);

        Task<IEnumerable<UserModel>> GetUserFriendsAsync(int id);

        Task<IEnumerable<FriendModel>> GetUserFriendRequestsAsync(int id);

        Task<FriendModel> AddUserFriendAsync(AddUserFriendModel addUserFriendModel);

        Task AcceptFriendRequestAsync(int id);

        Task RejectFriendRequestAsync(int id);
    }
}