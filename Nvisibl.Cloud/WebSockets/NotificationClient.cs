using Nvisibl.Cloud.WebSockets.Interfaces;
using Nvisibl.Cloud.WebSockets.Messages.Server;
using System;

namespace Nvisibl.Cloud.WebSockets
{
    public class NotificationClient : INotificationClient
    {
        private readonly WebSocketSession _webSocketSession;

        public NotificationClient(int userId, Guid sessionId, WebSocketSession webSocketSession)
        {
            UserId = userId;
            SessionId = sessionId;
            _webSocketSession = webSocketSession ?? throw new ArgumentNullException(nameof(webSocketSession));
        }

        public int UserId { get; }

        public Guid SessionId { get; }

        public void SendChatroomInvite(ChatroomInvitationMessage chatroomInvitation) =>
            _webSocketSession.EnqueueMessage(chatroomInvitation);

        public void SendFriendRequest(FriendRequestMessage friendRequest) =>
            _webSocketSession.EnqueueMessage(friendRequest);
    }
}
