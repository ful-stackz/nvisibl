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
    public class MessagesManagerService : IMessagesManagerService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public MessagesManagerService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory ?? throw new ArgumentNullException(nameof(unitOfWorkFactory));
        }

        public async Task<IEnumerable<MessageWithAuthorModel>> GetChatroomMessagesAsync(
            int chatroomId,
            int page,
            int pageSize)
        {
            if (chatroomId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(chatroomId), chatroomId, string.Empty);
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            var chatroom = await unitOfWork.GetRepository<IChatroomRepository>().GetAsync(chatroomId);
            if (chatroom is null)
            {
                throw new InvalidOperationException($"Chatroom with id ({chatroomId}) does not exist.");
            }

            return (await unitOfWork.GetRepository<IMessageRepository>()
                .GetChatroomMessagesAsync(chatroomId, page, pageSize))
                .Select(Mappers.ToMessageWithAuthorModel)
                .ToList();
        }

        public async Task<IEnumerable<MessageWithChatroomModel>> GetUserMessagesAsync(
            int userId,
            int page,
            int pageSize)
        {
            if (userId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), userId, string.Empty);
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            var user = await unitOfWork.GetRepository<IUserRepository>().GetAsync(userId);
            if (user is null)
            {
                throw new InvalidOperationException($"User with id ({userId}) does not exist.");
            }

            return (await unitOfWork.GetRepository<IMessageRepository>().GetUserMessagesAsync(userId, page, pageSize))
                .Select(Mappers.ToMessageWithChatroomModel)
                .ToList();
        }

        public async Task<MessageModel> CreateMessageAsync(CreateMessageModel createMessageModel)
        {
            if (createMessageModel is null)
            {
                throw new ArgumentNullException(nameof(createMessageModel));
            }

            using var unitOfWork = _unitOfWorkFactory.Create();
            var message = new Message
            {
                AuthorId = createMessageModel.AuthorId,
                Body = createMessageModel.Body,
                ChatroomId = createMessageModel.ChatroomId,
                TimeSentUtc = createMessageModel.TimeSentUtc,
            };
            await unitOfWork.GetRepository<IMessageRepository>().AddAsync(message);
            _ = await unitOfWork.CompleteAsync();
            return Mappers.ToMessageModel(message);
        }
    }
}
