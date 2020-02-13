using Nvisibl.Cloud.Models.Data;
using Nvisibl.Cloud.Services.Interfaces;
using Nvisibl.Cloud.WebSockets.Messages.Server;
using System;
using System.Reactive.Linq;

namespace Nvisibl.Cloud.WebSockets
{
    public class NotificationClient : IDisposable
    {
        private readonly IDisposable _notificationsSubscription;

        private bool _isDisposed;

        public NotificationClient(
            int userId,
            WebSocketSession webSocketSession,
            INotificationsService notificationsService)
        {
            if (webSocketSession is null)
            {
                throw new ArgumentNullException(nameof(webSocketSession));
            }

            if (notificationsService is null)
            {
                throw new ArgumentNullException(nameof(notificationsService));
            }

            _notificationsSubscription = notificationsService.Notifications
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
                    }));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _notificationsSubscription.Dispose();
                }
                _isDisposed = true;
            }
        }
    }
}
