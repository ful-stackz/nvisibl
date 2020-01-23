using Microsoft.Extensions.Logging;
using Nvisibl.Cloud.Factories.Interfaces;
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
    public class ChatroomManagerService : IChatroomManagerService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public ChatroomManagerService(IUnitOfWorkFactory unitOfWorkFactory, ILogger<ChatroomManagerService> logger)
        {
            _unitOfWorkFactory = unitOfWorkFactory ?? throw new ArgumentNullException(nameof(unitOfWorkFactory));
        }

        public async Task<ChatroomModel> CreateChatroomAsync(CreateChatroomModel createChatroomModel)
        {
            if (createChatroomModel is null)
            {
                throw new ArgumentNullException(nameof(createChatroomModel));
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            var owner = await unitOfWork.GetRepository<IUserRepository>().GetAsync(createChatroomModel.OwnerId);
            if (owner is null)
            {
                throw new InvalidOperationException($"User with id ({createChatroomModel.OwnerId}) does not exist.");
            }

            var chatroom = new Chatroom { Name = createChatroomModel.ChatroomName, };
            await unitOfWork.GetRepository<IChatroomRepository>().AddAsync(chatroom);
            _ = await unitOfWork.CompleteAsync();

            await unitOfWork.GetRepository<IChatroomUserRepository>().AddAsync(new ChatroomUser
            {
                ChatroomId = chatroom.Id,
                UserId = owner.Id,
            });
            _ = await unitOfWork.CompleteAsync();

            return Mappers.ToChatroomModel(chatroom);
        }

        public async Task<IEnumerable<ChatroomModel>> GetAllUserChatroomsAsync(UserModel user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            return (await unitOfWork.GetRepository<IChatroomRepository>()
                .GetAllUserChatroomsAsync(new User { Id = user.Id, }))
                .Select(Mappers.ToChatroomModel)
                .ToList();
        }

        public async Task<ChatroomModel> GetChatroomByIdAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            var chatroom = await unitOfWork.GetRepository<IChatroomRepository>().GetAsync(id);
            if (chatroom is null)
            {
                throw new InvalidOperationException($"Chatroom with id ({id}) does not exist.");
            }

            return Mappers.ToChatroomModel(chatroom);
        }

        public async Task<ChatroomWithUsersModel> GetChatroomByIdWithUsersAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            var chatroom = await unitOfWork.GetRepository<IChatroomRepository>().GetAsync(id);
            if (chatroom is null)
            {
                throw new InvalidOperationException($"Chatroom with id ({id}) does not exist.");
            }

            var users = await unitOfWork.GetRepository<IChatroomRepository>().GetAllChatroomUsers(id);
            return Mappers.ToChatroomWithUsersModel(chatroom, users);
        }

        public async Task<IEnumerable<ChatroomModel>> GetChatroomsAsync(int page = 0, int pageSize = 10)
        {
            if (page < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(page), page, string.Empty);
            }

            if (pageSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, string.Empty);
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            return (await unitOfWork.GetRepository<IChatroomRepository>().GetRangeAsync(page, pageSize))
                .Select(Mappers.ToChatroomModel)
                .ToList();
        }

        public async Task DeleteChatroomAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            var chatroom = await unitOfWork.GetRepository<IChatroomRepository>().GetAsync(id);
            if (chatroom is null)
            {
                throw new InvalidOperationException($"Chatroom with id ({id}) does not exist.");
            }

            unitOfWork.GetRepository<IChatroomRepository>().Remove(chatroom);
            _ = await unitOfWork.CompleteAsync();
        }

        public async Task AddUserToChatroomAsync(int chatroomId, UserModel user)
        {
            if (chatroomId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(chatroomId), chatroomId, string.Empty);
            }

            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            await unitOfWork.GetRepository<IChatroomUserRepository>().AddAsync(new ChatroomUser
            {
                ChatroomId = chatroomId,
                UserId = user.Id,
            });
            _ = await unitOfWork.CompleteAsync();
        }
    }
}
