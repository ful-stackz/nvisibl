using Microsoft.Extensions.Logging;
using Nvisibl.Cloud.Factories.Interfaces;
using Nvisibl.Cloud.Models;
using Nvisibl.Cloud.Services.Interfaces;
using Nvisibl.DataLibrary.Models;
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
            var owner = await unitOfWork.UserRepository.GetAsync(createChatroomModel.OwnerId);
            var chatroom = new Chatroom { Name = createChatroomModel.ChatroomName, };
            await unitOfWork.ChatroomRepository.AddAsync(chatroom);
            _ = await unitOfWork.CompleteAsync();
            await unitOfWork.ChatroomUserRepository.AddAsync(new ChatroomUser
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
            var chatrooms = await unitOfWork.ChatroomRepository.GetAllUserChatroomsAsync(new User { Id = user.Id, });
            return chatrooms.Select(Mappers.ToChatroomModel).ToList();
        }

        public async Task<ChatroomModel> GetChatroomByIdAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            using var unitOfWork = _unitOfWorkFactory.Create();
            var chatroom = await unitOfWork.ChatroomRepository.GetAsync(id);
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
            var chatroom = await unitOfWork.ChatroomRepository.GetAsync(id);
            if (chatroom is null)
            {
                throw new InvalidOperationException($"Chatroom with id ({id}) does not exist.");
            }

            var users = await unitOfWork.ChatroomRepository.GetAllChatroomUsers(id);
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
            return (await unitOfWork.ChatroomRepository.GetRangeAsync(page, pageSize))
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
            var chatroom = await unitOfWork.ChatroomRepository.GetAsync(id);
            if (chatroom is null)
            {
                throw new InvalidOperationException($"Chatroom with id ({id}) does not exist.");
            }

            unitOfWork.ChatroomRepository.Remove(chatroom);
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
            await unitOfWork.ChatroomUserRepository.AddAsync(new ChatroomUser
            {
                ChatroomId = chatroomId,
                UserId = user.Id,
            });
            _ = await unitOfWork.CompleteAsync();
        }
    }
}
