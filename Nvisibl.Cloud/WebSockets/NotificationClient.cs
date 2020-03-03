using Nvisibl.Business.Interfaces;
using Nvisibl.Cloud.Models.Data;
using Nvisibl.Cloud.Services.Interfaces;
using Nvisibl.Cloud.WebSockets.Interfaces;
using Nvisibl.Cloud.WebSockets.Messages.Server;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Nvisibl.Cloud.WebSockets
{
    public class NotificationClient : INotificationClient, IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        private bool _isDisposed;

        public NotificationClient(
            int userId,
            Guid sessionId,
            WebSocketSession webSocketSession,
            IChatroomsManager chatroomsManager,
            INotificationsService notificationsService)
        {
            if (webSocketSession is null)
            {
                throw new ArgumentNullException(nameof(webSocketSession));
            }

            if (chatroomsManager is null)
            {
                throw new ArgumentNullException(nameof(chatroomsManager));
            }

            if (notificationsService is null)
            {
                throw new ArgumentNullException(nameof(notificationsService));
            }

            UserId = userId;
            SessionId = sessionId;

            var chatrooms = chatroomsManager.GetUserChatroomsAsync(new Business.Models.Users.UserModel { Id = userId })
                .GetAwaiter()
                .GetResult()
                .Select(chatroom => chatroom.Id)
                .ToList();

            _subscriptions.Add(
                notificationsService.Notifications
                .Where(notification => notification is ChatroomMessageNotification)
                .OfType<ChatroomMessageNotification>()
                .Select(notification => notification.Message)
                .Where(msg => chatrooms.Contains(msg.ChatroomId))
                .Subscribe(msg => webSocketSession.EnqueueMessage(new ChatroomMessageMessage
                {
                    AuthorId = msg.AuthorId,
                    Body = msg.Body,
                    ChatroomId = msg.ChatroomId,
                    MessageId = msg.Id,
                    TimeSentUtc = msg.TimeSentUtc.ToString("o"),
                })));

            _subscriptions.Add(
                notificationsService.Notifications
                .Where(notification => notification is ChatroomChangedNotification)
                .OfType<ChatroomChangedNotification>()
                .Where(notification =>
                    notification.Participants.Contains(userId) && !chatrooms.Contains(notification.ChatroomId))
                .Subscribe(notification =>
                {
                    chatrooms.Add(notification.ChatroomId);
                    webSocketSession.EnqueueMessage(new ChatroomInvitationMessage
                    {
                        ChatroomId = notification.ChatroomId,
                        ChatroomName = notification.ChatroomName,
                        Users = notification.Participants,
                    });
                }));

            _subscriptions.Add(
                notificationsService.Notifications
                .Where(notification => notification is FriendRequestNotification)
                .OfType<FriendRequestNotification>()
                .Where(notification => notification.Sender.Id == userId || notification.Receiver.Id == userId)
                .Subscribe(notification => webSocketSession.EnqueueMessage(
                    new FriendRequestMessage
                    {
                        Accepted = notification.Accepted,
                        Id = notification.FriendRequestId,
                        ReceiverId = notification.Receiver.Id,
                        ReceiverUsername = notification.Receiver.Username,
                        SenderId = notification.Sender.Id,
                        SenderUsername = notification.Sender.Username,
                    })));
        }

        public int UserId { get; }

        public Guid SessionId { get; }

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
