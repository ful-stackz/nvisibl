using Nvisibl.Cloud.Models;
using Nvisibl.Cloud.Models.Users;
using Nvisibl.Cloud.Services.Interfaces;
using Nvisibl.DataLibrary.Models;
using Nvisibl.DataLibrary.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Services
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

        public async Task AddUserFriendAsync(UserModel userModel, UserModel friendUserModel)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (userModel is null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            if (friendUserModel is null)
            {
                throw new ArgumentNullException(nameof(friendUserModel));
            }

            await EnsureUserExistsAsync(userModel.Id);
            await EnsureUserExistsAsync(friendUserModel.Id);

            await _unitOfWork.GetRepository<IFriendsRepository>().AddAsync(new Friend
            {
                User1Id = userModel.Id,
                User2Id = friendUserModel.Id,
            });
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
