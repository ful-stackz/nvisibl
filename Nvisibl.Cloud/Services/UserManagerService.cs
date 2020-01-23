using Nvisibl.Cloud.Models;
using Nvisibl.Cloud.Services.Interfaces;
using Nvisibl.DataLibrary.Models;
using Nvisibl.DataLibrary.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly IUnitOfWork _unitOfWork;

        private bool _isDisposed;

        public UserManagerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task MakeFriendsAsync(int userId, UserModel friend)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            if (friend is null)
            {
                throw new ArgumentNullException(nameof(friend));
            }

            await _unitOfWork.GetRepository<IFriendsRepository>().AddAsync(new Friend
            {
                User1Id = userId,
                User2Id = friend.Id,
            });
            _ = await _unitOfWork.CompleteAsync();
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

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (id <= 0)
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

        public async Task<IEnumerable<UserModel>> GetFriendsAsync(int userId)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (userId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            var user = await _unitOfWork.GetRepository<IUserRepository>().GetAsync(userId);
            if (user is null)
            {
                throw new InvalidOperationException($"User with id ({userId}) does not exist.");
            }

            return (await _unitOfWork.GetRepository<IFriendsRepository>().GetAllFriendsAsync(user))
                .Select(Mappers.ToUserModel)
                .ToList();
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
    }
}
