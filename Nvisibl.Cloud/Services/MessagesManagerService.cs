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
    public class MessagesManagerService : IMessagesManagerService, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        private bool _isDisposed;

        public MessagesManagerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<MessageWithAuthorModel>> GetChatroomMessagesAsync(
            int chatroomId,
            int page,
            int pageSize)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (chatroomId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(chatroomId), chatroomId, string.Empty);
            }

            var chatroom = await _unitOfWork.GetRepository<IChatroomRepository>().GetAsync(chatroomId);
            if (chatroom is null)
            {
                throw new InvalidOperationException($"Chatroom with id ({chatroomId}) does not exist.");
            }

            return (await _unitOfWork.GetRepository<IMessageRepository>()
                .GetChatroomMessagesAsync(chatroomId, page, pageSize))
                .Select(Mappers.ToMessageWithAuthorModel)
                .ToList();
        }

        public async Task<IEnumerable<MessageWithChatroomModel>> GetUserMessagesAsync(
            int userId,
            int page,
            int pageSize)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (userId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), userId, string.Empty);
            }

            var user = await _unitOfWork.GetRepository<IUserRepository>().GetAsync(userId);
            if (user is null)
            {
                throw new InvalidOperationException($"User with id ({userId}) does not exist.");
            }

            return (await _unitOfWork.GetRepository<IMessageRepository>().GetUserMessagesAsync(userId, page, pageSize))
                .Select(Mappers.ToMessageWithChatroomModel)
                .ToList();
        }

        public async Task<MessageModel> CreateMessageAsync(CreateMessageModel createMessageModel)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (createMessageModel is null)
            {
                throw new ArgumentNullException(nameof(createMessageModel));
            }

            var message = new Message
            {
                AuthorId = createMessageModel.AuthorId,
                Body = createMessageModel.Body,
                ChatroomId = createMessageModel.ChatroomId,
                TimeSentUtc = createMessageModel.TimeSentUtc,
            };
            await _unitOfWork.GetRepository<IMessageRepository>().AddAsync(message);
            _ = await _unitOfWork.CompleteAsync();
            return Mappers.ToMessageModel(message);
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
