using Nvisibl.Business.Models;
using Nvisibl.Business.Models.Users;
using Nvisibl.Business.Interfaces;
using Nvisibl.DataLibrary.Models;
using Nvisibl.DataLibrary.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nvisibl.Business
{
    public class UsersManager : IUsersManager
    {
        private readonly IUnitOfWork _unitOfWork;

        private bool _isDisposed;

        public UsersManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<UserModel> CreateUserAsync(CreateUserModel createUserModel)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (createUserModel is null)
            {
                throw new ArgumentNullException(nameof(createUserModel));
            }

            var user = new User { Username = createUserModel.Username, };
            await _unitOfWork.GetRepository<IUserRepository>().AddAsync(user);
            _ = await _unitOfWork.CompleteAsync();

            return Mappers.ToUserModel(user);
        }

        public async Task<FriendModel> CreateFriendRequestAsync(AddUserFriendModel addUserFriendModel)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (addUserFriendModel is null)
            {
                throw new ArgumentNullException(nameof(addUserFriendModel));
            }

            var friendsRepository = _unitOfWork.GetRepository<IFriendsRepository>();
            bool friendRequestExists = friendsRepository.Find(req =>
                (req.User1Id == addUserFriendModel.UserId && req.User2Id == addUserFriendModel.FriendId) ||
                (req.User1Id == addUserFriendModel.FriendId && req.User2Id == addUserFriendModel.UserId))
                .Any();
            if (friendRequestExists)
            {
                throw new InvalidOperationException("Friend request already exists.");
            }

            var friendRequest = new Friend
            {
                Accepted = false,
                User1Id = addUserFriendModel.UserId,
                User2Id = addUserFriendModel.FriendId,
            };
            await _unitOfWork.GetRepository<IFriendsRepository>().AddAsync(friendRequest);
            _ = await _unitOfWork.CompleteAsync();

            return new FriendModel
            {
                Accepted = friendRequest.Accepted,
                Id = friendRequest.Id,
                User1Id = friendRequest.User1Id,
                User2Id = friendRequest.User2Id,
            };
        }

        public async Task<UserModel> GetUserAsync(int id)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return Mappers.ToUserModel(await _unitOfWork.GetRepository<IUserRepository>().GetAsync(id));
        }

        public async Task<UserModel?> GetUserAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Argument was null or empty.", nameof(username));
            }

            return await Task.Run(() =>
            {
                var user = _unitOfWork.GetRepository<IUserRepository>()
                    .Find(u => u.Username == username)
                    .FirstOrDefault();
                return user is null ? null : Mappers.ToUserModel(user);
            });
        }

        public async Task<FriendModel> GetFriendRequestAsync(int id)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            var friendRequest = await _unitOfWork.GetRepository<IFriendsRepository>().GetAsync(id);
            return new FriendModel
            {
                Accepted = friendRequest.Accepted,
                Id = friendRequest.Id,
                User1Id = friendRequest.User1Id,
                User2Id = friendRequest.User2Id,
            };
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(int page = 0, int pageSize = 10)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (page < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(page));
            }

            if (pageSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            return (await _unitOfWork.GetRepository<IUserRepository>().GetRangeAsync(page, pageSize))
                .Select(Mappers.ToUserModel)
                .ToList();
        }

        public async Task<IEnumerable<UserModel>> GetUserFriendsAsync(int id)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            await EnsureUserExistsAsync(id);

            return (await _unitOfWork.GetRepository<IFriendsRepository>().GetAllFriendsAsync(new User { Id = id, }))
                .Select(Mappers.ToUserModel)
                .ToList();
        }

        public async Task<IEnumerable<FriendModel>> GetUserFriendRequestsAsync(int id)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            return await Task.Run(() =>
                _unitOfWork.GetRepository<IFriendsRepository>()
                .Find(f => (f.User1Id == id || f.User2Id == id) && !f.Accepted, maxCount: 50)
                .Select(f => new FriendModel
                {
                    Accepted = f.Accepted,
                    Id = f.Id,
                    User1Id = f.User1Id,
                    User2Id = f.User2Id,
                })
                .ToList());
        }

        public async Task<FriendModel> AddUserFriendAsync(AddUserFriendModel addUserFriendModel)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (addUserFriendModel is null)
            {
                throw new ArgumentNullException(nameof(addUserFriendModel));
            }

            await EnsureUserExistsAsync(addUserFriendModel.UserId);
            await EnsureUserExistsAsync(addUserFriendModel.FriendId);

            var friend = new Friend
            {
                Accepted = true,
                User1Id = addUserFriendModel.UserId,
                User2Id = addUserFriendModel.FriendId,
            };
            await _unitOfWork.GetRepository<IFriendsRepository>().AddAsync(friend);
            _ = await _unitOfWork.CompleteAsync();

            return new FriendModel
            {
                Accepted = friend.Accepted,
                Id = friend.Id,
                User1Id = friend.User1Id,
                User2Id = friend.User2Id,
            };
        }

        public async Task AcceptFriendRequestAsync(int id)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            var friendsRepository = _unitOfWork.GetRepository<IFriendsRepository>();
            var friendRequest = await friendsRepository.GetAsync(id);
            if (friendRequest is null)
            {
                throw new InvalidOperationException($"Friend request with id ({id}) does not exist.");
            }

            friendRequest.Accepted = true;
            friendsRepository.Update(friendRequest);
            _ = await _unitOfWork.CompleteAsync();
        }

        public async Task RejectFriendRequestAsync(int id)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            var friendsRepository = _unitOfWork.GetRepository<IFriendsRepository>();
            var friendRequest = await friendsRepository.GetAsync(id);
            if (friendRequest is null)
            {
                throw new InvalidOperationException($"Friend request with id ({id}) does not exist.");
            }

            friendsRepository.Remove(friendRequest);
            _ = await _unitOfWork.CompleteAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                if (isDisposing)
                {
                    _unitOfWork.Dispose();
                }
                _isDisposed = true;
            }
        }

        private async Task EnsureUserExistsAsync(int id)
        {
            if (await _unitOfWork.GetRepository<IUserRepository>().GetAsync(id) is null)
            {
                throw new InvalidOperationException($"User with id ({id}) does not exist.");
            }
        }
    }
}
