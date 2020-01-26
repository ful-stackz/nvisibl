using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nvisibl.Cloud.WebSockets;
using Nvisibl.Cloud.WebSockets.Interfaces;
using Nvisibl.Cloud.WebSockets.Messages.Client;
using Nvisibl.Cloud.WebSockets.Messages.Client.Base;
using Nvisibl.Cloud.WebSockets.Messages.Interfaces;
using Nvisibl.Cloud.WebSockets.Messages.Server;

namespace Nvisibl.Cloud.Middleware
{
    public class WebSocketsMiddleware
    {
        private static readonly TimeSpan DefaultConnectionTimeout = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan DevConnectionTimeout = TimeSpan.FromSeconds(15);

        private const string ChatClientEndpoint = "/ws/chat";

        private readonly RequestDelegate _next;

        public WebSocketsMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(
            HttpContext httpContext,
            IWebHostEnvironment hostEnvironment,
            IChatClientManager chatClientManager,
            IMessageParser messageParser,
            ILogger<WebSocketSession> logger)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (!httpContext.WebSockets.IsWebSocketRequest || httpContext.Request.Path != ChatClientEndpoint)
            {
                await _next(httpContext);
                return;
            }

            if (hostEnvironment is null)
            {
                throw new ArgumentNullException(nameof(hostEnvironment));
            }

            if (chatClientManager is null)
            {
                throw new ArgumentNullException(nameof(chatClientManager));
            }

            if (messageParser is null)
            {
                throw new ArgumentNullException(nameof(messageParser));
            }

            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();

            using var webSocketSession = new WebSocketSession(webSocket, messageParser, logger);

            var connectionRequest = webSocketSession.ReceivedMessages
                .Where(msg => msg is ConnectionRequestMessage)
                .Timeout(
                    hostEnvironment.IsDevelopment() ? DevConnectionTimeout : DefaultConnectionTimeout,
                    Observable.Return(ClientMessageBase.Empty))
                .Take(1)
                .Wait() as ConnectionRequestMessage;

            if (connectionRequest is null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.CompleteAsync();
                return;
            }

            var sessionId = Guid.NewGuid();
            var chatClient = new ChatClient(connectionRequest.UserId, sessionId, webSocketSession);

            webSocketSession.EnqueueMessage(new ConnectedMessage { SessionId = sessionId, });

            chatClientManager.AddClient(chatClient);

            await webSocketSession.Lifetime.Task;

            chatClientManager.RemoveClient(client => client.UserId == chatClient.UserId);
        }
    }
}
