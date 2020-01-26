using Nvisibl.Cloud.Models.Messages;
using Nvisibl.Cloud.Models.Users;
using Nvisibl.Cloud.Services.Interfaces;
using Nvisibl.Cloud.WebSockets.Interfaces;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Nvisibl.Cloud.WebSockets
{
    public class ChatClient : IChatClient, IDisposable
    {
        private readonly WebSocketSession _webSocketSession;
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        private bool _isDisposed;

        public ChatClient(
            int userId,
            Guid sessionId,
            WebSocketSession webSocketSession,
            IChatroomsManager chatroomsManager,
            IMessengerService messengerService)
        {
            if (chatroomsManager is null)
            {
                throw new ArgumentNullException(nameof(chatroomsManager));
            }

            if (messengerService is null)
            {
                throw new ArgumentNullException(nameof(messengerService));
            }

            UserId = userId;
            SessionId = sessionId;
            _webSocketSession = webSocketSession ?? throw new ArgumentNullException(nameof(webSocketSession));

            _subscriptions.Add(
                webSocketSession.ReceivedMessages
                .Where(msg => msg is Messages.Client.ChatroomMessageMessage)
                .OfType<Messages.Client.ChatroomMessageMessage>()
                .Subscribe(async msg => await messengerService.SendMessageAsync(new CreateMessageModel
                {
                    AuthorId = msg.AuthorId,
                    Body = msg.Body,
                    ChatroomId = msg.ChatroomId,
                    TimeSentUtc = msg.TimeSentUtc,
                })));

            var chatrooms = chatroomsManager.GetUserChatroomsAsync(new UserModel { Id = userId })
                .GetAwaiter()
                .GetResult()
                .Select(chatroom => chatroom.Id)
                .ToList();
            _subscriptions.Add(
                messengerService.DispatchedMessages
                .Where(msg => msg.AuthorId != UserId && !chatrooms.Contains(msg.ChatroomId))
                .Subscribe(msg => webSocketSession.EnqueueMessage(new Messages.Server.ChatroomMessageMessage
                {
                    AuthorId = msg.AuthorId,
                    Body = msg.Body,
                    ChatroomId = msg.ChatroomId,
                    MessageId = msg.Id,
                    TimeSentUtc = msg.TimeSentUtc,
                })));
        }

        public int UserId { get; }

        public Guid SessionId { get; }

        public void SendMessage(MessageModel message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _webSocketSession.EnqueueMessage(new Messages.Server.ChatroomMessageMessage
            {
                AuthorId = message.AuthorId,
                Body = message.Body,
                ChatroomId = message.ChatroomId,
                MessageId = message.Id,
                TimeSentUtc = message.TimeSentUtc,
            });
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
                    _subscriptions.Dispose();
                }
                _isDisposed = true;
            }
        }
    }
}
