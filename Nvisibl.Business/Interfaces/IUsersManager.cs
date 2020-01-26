using Nvisibl.Business.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nvisibl.Business.Interfaces
{
    public interface IUsersManager
    {
        Task<UserModel> CreateUserAsync(CreateUserModel createUserModel);

        Task<UserModel> GetUserAsync(int id);

        Task<IEnumerable<UserModel>> GetUsersAsync(int page, int pageSize);

        Task<IEnumerable<UserModel>> GetUserFriendsAsync(int id);

        Task AddUserFriendAsync(AddUserFriendModel addUserFriendModel);
    }
}