using Nvisibl.Business.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nvisibl.Business.Interfaces
{
    public interface IUsersManager
    {
        /// <exception cref="System.ArgumentNullException">
        /// When one of the required arguments is <c>null</c>.
        /// </exception>
        Task<UserModel> CreateUserAsync(CreateUserModel createUserModel);

        /// <exception cref="System.ArgumentNullException">
        /// When one of the required arguments is <c>null</c>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// When the friend request already exists.
        /// </exception>
        Task<FriendModel> CreateFriendRequestAsync(AddUserFriendModel addUserFriendModel);

        /// <exception cref="System.ArgumentOutOfRangeException">
        /// When one of the arguments is out of range.
        /// </exception>
        Task<UserModel> GetUserAsync(int id);

        Task<UserModel?> GetUserAsync(string username);

        /// <exception cref="System.ArgumentOutOfRangeException">
        /// When one of the arguments is out of range.
        /// </exception>
        Task<FriendModel> GetFriendRequestAsync(int id);

        /// <exception cref="System.ArgumentOutOfRangeException">
        /// When one of the arguments is out of range.
        /// </exception>
        Task<IEnumerable<UserModel>> GetUsersAsync(int page, int pageSize);

        /// <exception cref="System.ArgumentOutOfRangeException">
        /// When one of the arguments is out of range.
        /// </exception>
        Task<IEnumerable<UserModel>> GetUserFriendsAsync(int id);

        /// <exception cref="System.ArgumentOutOfRangeException">
        /// When one of the arguments is out of range.
        /// </exception>
        Task<IEnumerable<FriendModel>> GetUserFriendRequestsAsync(int id);

        /// <exception cref="System.ArgumentNullException">
        /// When one of the required arguments is <c>null</c>.
        /// </exception>
        Task<FriendModel> AddUserFriendAsync(AddUserFriendModel addUserFriendModel);

        /// <exception cref="System.ArgumentOutOfRangeException">
        /// When one of the arguments is out of range.
        /// </exception>
        Task AcceptFriendRequestAsync(int id);

        /// <exception cref="System.ArgumentOutOfRangeException">
        /// When one of the arguments is out of range.
        /// </exception>
        Task RejectFriendRequestAsync(int id);
    }
}