using Nvisibl.Cloud.Models;
using Nvisibl.Cloud.Models.Chatrooms;
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
    public class ChatroomsManager : IChatroomsManager, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        private bool _isDisposed;

        public ChatroomsManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ChatroomModel> CreateChatroomAsync(CreateChatroomModel createChatroomModel)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (createChatroomModel is null)
            {
                throw new ArgumentNullException(nameof(createChatroomModel));
            }

            await EnsureUserExistsAsync(createChatroomModel.OwnerId);

            var chatroom = new Chatroom { Name = createChatroomModel.ChatroomName, };
            await _unitOfWork.GetRepository<IChatroomRepository>().AddAsync(chatroom);
            _ = await _unitOfWork.CompleteAsync();

            await _unitOfWork.GetRepository<IChatroomUserRepository>().AddAsync(new ChatroomUser
            {
                ChatroomId = chatroom.Id,
                UserId = createChatroomModel.OwnerId,
            });
            _ = await _unitOfWork.CompleteAsync();

            return Mappers.ToChatroomModel(chatroom);
        }

        public async Task<ChatroomModel> GetChatroomAsync(int id)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            var chatroom = await _unitOfWork.GetRepository<IChatroomRepository>().GetAsync(id);
            if (chatroom is null)
            {
                throw new InvalidOperationException($"Chatroom with id ({id}) does not exist.");
            }

            return Mappers.ToChatroomModel(chatroom);
        }

        public async Task<IEnumerable<ChatroomModel>> GetChatroomsAsync(int page = 0, int pageSize = 10)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (page < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(page), page, string.Empty);
            }

            if (pageSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, string.Empty);
            }

            return (await _unitOfWork.GetRepository<IChatroomRepository>().GetRangeAsync(page, pageSize))
                .Select(Mappers.ToChatroomModel)
                .ToList();
        }

        public async Task DeleteChatroomAsync(int id)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            var chatroom = await _unitOfWork.GetRepository<IChatroomRepository>().GetAsync(id);
            if (chatroom is null)
            {
                throw new InvalidOperationException($"Chatroom with id ({id}) does not exist.");
            }

            _unitOfWork.GetRepository<IChatroomRepository>().Remove(chatroom);
            _ = await _unitOfWork.CompleteAsync();
        }

        public async Task AddUserToChatroomAsync(AddUserToChatroomModel addUserToChatroomModel)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (addUserToChatroomModel is null)
            {
                throw new ArgumentNullException(nameof(addUserToChatroomModel));
            }

            await EnsureUserExistsAsync(addUserToChatroomModel.UserId);
            await EnsureChatroomExistsAsync(addUserToChatroomModel.ChatroomId);

            await _unitOfWork.GetRepository<IChatroomUserRepository>().AddAsync(new ChatroomUser
            {
                ChatroomId = addUserToChatroomModel.ChatroomId,
                UserId = addUserToChatroomModel.UserId,
            });
            _ = await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<ChatroomModel>> GetUserChatroomsAsync(UserModel userModel)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (userModel is null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            await EnsureUserExistsAsync(userModel.Id);

            return (await _unitOfWork.GetRepository<IChatroomRepository>()
                .GetAllUserChatroomsAsync(new User { Id = userModel.Id, }))
                .Select(Mappers.ToChatroomModel)
                .ToList();
        }

        public async Task<IEnumerable<UserModel>> GetChatroomUsersAsync(int id)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            await EnsureChatroomExistsAsync(id);

            var users = await _unitOfWork.GetRepository<IChatroomRepository>().GetAllChatroomUsers(id);
            return users.Select(Mappers.ToUserModel);
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

        private async Task EnsureChatroomExistsAsync(int id)
        {
            if (await _unitOfWork.GetRepository<IChatroomRepository>().GetAsync(id) is null)
            {
                throw new InvalidOperationException($"Chatroom with id ({id}) does not exist.");
            }
        }
    }
}
