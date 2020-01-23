using Nvisibl.Cloud.WebSockets.Interfaces;
using Nvisibl.Cloud.WebSockets.Messages.Client;
using System;
using System.Reactive.Linq;

namespace Nvisibl.Cloud.WebSockets
{
    public class ChatClient : IChatClient
    {
        private readonly WebSocketSession _webSocketSession;

        public ChatClient(int userId, Guid sessionId, WebSocketSession webSocketSession)
        {
            UserId = userId;
            SessionId = sessionId;
            _webSocketSession = webSocketSession ?? throw new ArgumentNullException(nameof(webSocketSession));
            ReceivedMessages = webSocketSession.ReceivedMessages
                .Where(msg => msg is ChatroomMessageMessage)
                .OfType<ChatroomMessageMessage>()
                .AsObservable();
        }

        public IObservable<ChatroomMessageMessage> ReceivedMessages { get; }

        public int UserId { get; }

        public Guid SessionId { get; }

        public void SendMessage(Messages.Server.ChatroomMessageMessage message)
        {
            _webSocketSession.EnqueueMessage(message);
        }
    }
}
