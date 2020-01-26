using Nvisibl.Business.Models;
using Nvisibl.Business.Models.Messages;
using Nvisibl.Business.Interfaces;
using Nvisibl.DataLibrary.Models;
using Nvisibl.DataLibrary.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nvisibl.Business
{
    public class MessagesManager : IMessagesManager, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        private bool _isDisposed;

        public MessagesManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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

        public async Task<IEnumerable<MessageModel>> GetChatroomMessagesAsync(
            int id,
            int page,
            int pageSize)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            if (page < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(page), page, string.Empty);
            }

            if (pageSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, string.Empty);
            }

            var chatroom = await _unitOfWork.GetRepository<IChatroomRepository>().GetAsync(id);
            if (chatroom is null)
            {
                throw new InvalidOperationException($"Chatroom with id ({id}) does not exist.");
            }

            return (await _unitOfWork.GetRepository<IMessageRepository>()
                .GetChatroomMessagesAsync(id, page, pageSize))
                .Select(Mappers.ToMessageModel)
                .ToList();
        }

        public async Task<IEnumerable<MessageModel>> GetUserMessagesAsync(
            int id,
            int page,
            int pageSize)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, string.Empty);
            }

            if (page < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(page), page, string.Empty);
            }

            if (pageSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, string.Empty);
            }

            var user = await _unitOfWork.GetRepository<IUserRepository>().GetAsync(id);
            if (user is null)
            {
                throw new InvalidOperationException($"User with id ({id}) does not exist.");
            }

            return (await _unitOfWork.GetRepository<IMessageRepository>().GetUserMessagesAsync(id, page, pageSize))
                .Select(Mappers.ToMessageModel)
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
