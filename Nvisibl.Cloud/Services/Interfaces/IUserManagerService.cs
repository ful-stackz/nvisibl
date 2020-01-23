using Nvisibl.Cloud.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Services.Interfaces
{
    public interface IUserManagerService
    {
        Task<UserModel> CreateUserAsync(CreateUserModel createUserModel);
        Task<UserModel> GetUserByIdAsync(int id);
        Task<IEnumerable<UserModel>> GetUsersAsync(int rangeOffset = 0, int rangeSize = 10);
        Task<UserWithFriendsModel> GetUserWithFriendsByIdAsync(int id);
        Task MakeFriendsAsync(int userId, UserModel friend);
    }
}