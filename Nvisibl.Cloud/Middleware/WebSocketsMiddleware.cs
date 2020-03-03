using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Nvisibl.Business.Interfaces;
using Nvisibl.Cloud.Authentication;
using Nvisibl.Cloud.Services.Interfaces;
using Nvisibl.Cloud.WebSockets;
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

        private const string ChatClientEndpoint = "/ws";
        private const string AuthorizationHeaderKey = "Authorization";

        private readonly RequestDelegate _next;
        private readonly INotificationsService _notificationsService;
        private readonly IMessageParser _messageParser;
        private readonly TimeSpan _connectionTimeout;

        public WebSocketsMiddleware(
            RequestDelegate next,
            IWebHostEnvironment hostEnvironment,
            INotificationsService notificationsService,
            IMessageParser messageParser)
        {
            if (hostEnvironment is null)
            {
                throw new ArgumentNullException(nameof(hostEnvironment));
            }

            _next = next ?? throw new ArgumentNullException(nameof(next));
            _notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
            _messageParser = messageParser ?? throw new ArgumentNullException(nameof(messageParser));
            _connectionTimeout = hostEnvironment.IsDevelopment() ? DevConnectionTimeout : DefaultConnectionTimeout;
        }

        public async Task Invoke(
            HttpContext httpContext,
            IChatroomsManager chatroomsManager,
            ILogger<WebSocketSession> logger)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (chatroomsManager is null)
            {
                throw new ArgumentNullException(nameof(chatroomsManager));
            }

            if (!httpContext.WebSockets.IsWebSocketRequest || httpContext.Request.Path != ChatClientEndpoint)
            {
                await _next(httpContext);
                return;
            }

            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();

            using var webSocketSession = new WebSocketSession(webSocket, _messageParser, logger);

            var connectionRequest = webSocketSession.ReceivedMessages
                .Where(msg => msg is ConnectionRequestMessage)
                .Timeout(_connectionTimeout, Observable.Return(ClientMessageBase.Empty))
                .Take(1)
                .Wait() as ConnectionRequestMessage;

            if (connectionRequest is null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.CompleteAsync();
                return;
            }

            httpContext.Request.Headers.Add(GetAuthorizationHeader(connectionRequest));
            var authResults = await Task.WhenAll(
                httpContext.AuthenticateAsync(JwtSchemes.Admin),
                httpContext.AuthenticateAsync(JwtSchemes.User));
            if (authResults.All(result => !result.Succeeded))
            {
                await httpContext.Response.CompleteAsync();
                return;
            }

            var sessionId = Guid.NewGuid();
            using var chatClient = new NotificationClient(
                connectionRequest.UserId,
                sessionId,
                webSocketSession,
                chatroomsManager,
                _notificationsService);

            webSocketSession.EnqueueMessage(new ConnectedMessage { SessionId = sessionId, });

            await webSocketSession.Lifetime.Task;
        }

        private KeyValuePair<string, StringValues> GetAuthorizationHeader(ConnectionRequestMessage request) =>
            new KeyValuePair<string, StringValues>(AuthorizationHeaderKey, $"Bearer {request.AccessToken}");
    }
}
