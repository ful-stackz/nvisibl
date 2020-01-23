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
    public class ChatroomManagerService : IChatroomManagerService, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        private bool _isDisposed;

        public ChatroomManagerService(IUnitOfWork unitOfWork)
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

            var owner = await _unitOfWork.GetRepository<IUserRepository>().GetAsync(createChatroomModel.OwnerId);
            if (owner is null)
            {
                throw new InvalidOperationException($"User with id ({createChatroomModel.OwnerId}) does not exist.");
            }

            var chatroom = new Chatroom { Name = createChatroomModel.ChatroomName, };
            await _unitOfWork.GetRepository<IChatroomRepository>().AddAsync(chatroom);
            _ = await _unitOfWork.CompleteAsync();

            await _unitOfWork.GetRepository<IChatroomUserRepository>().AddAsync(new ChatroomUser
            {
                ChatroomId = chatroom.Id,
                UserId = owner.Id,
            });
            _ = await _unitOfWork.CompleteAsync();

            return Mappers.ToChatroomModel(chatroom);
        }

        public async Task<IEnumerable<ChatroomModel>> GetAllUserChatroomsAsync(UserModel user)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return (await _unitOfWork.GetRepository<IChatroomRepository>()
                .GetAllUserChatroomsAsync(new User { Id = user.Id, }))
                .Select(Mappers.ToChatroomModel)
                .ToList();
        }

        public async Task<ChatroomModel> GetChatroomByIdAsync(int id)
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

        public async Task<ChatroomWithUsersModel> GetChatroomByIdWithUsersAsync(int id)
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

            var users = await _unitOfWork.GetRepository<IChatroomRepository>().GetAllChatroomUsers(id);
            return Mappers.ToChatroomWithUsersModel(chatroom, users);
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

        public async Task AddUserToChatroomAsync(int chatroomId, UserModel user)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (chatroomId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(chatroomId), chatroomId, string.Empty);
            }

            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _unitOfWork.GetRepository<IChatroomUserRepository>().AddAsync(new ChatroomUser
            {
                ChatroomId = chatroomId,
                UserId = user.Id,
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
    }
}
