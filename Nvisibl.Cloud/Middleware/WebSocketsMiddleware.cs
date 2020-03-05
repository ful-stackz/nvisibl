using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.JsonWebTokens;
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
        private const string ChatClientEndpoint = "/ws";
        private const string AuthorizationHeaderKey = "Authorization";
        private const string ConnectionTimeoutKey = "WS_CONN_TIMEOUT";

        private static readonly TimeSpan DefaultConnectionTimeout = TimeSpan.FromSeconds(5);

        private readonly RequestDelegate _next;
        private readonly INotificationsService _notificationsService;
        private readonly IMessageParser _messageParser;
        private readonly ILogger<WebSocketsMiddleware> _logger;
        private readonly TimeSpan _connectionTimeout;

        public WebSocketsMiddleware(
            RequestDelegate next,
            IConfiguration configuration,
            INotificationsService notificationsService,
            IMessageParser messageParser,
            ILogger<WebSocketsMiddleware> logger)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _next = next ?? throw new ArgumentNullException(nameof(next));
            _notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
            _messageParser = messageParser ?? throw new ArgumentNullException(nameof(messageParser));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionTimeout = configuration.GetValue<int?>(ConnectionTimeoutKey, null) is { } timeoutInSec
                ? TimeSpan.FromSeconds(timeoutInSec)
                : DefaultConnectionTimeout;
        }

        public async Task Invoke(
            HttpContext httpContext,
            IChatroomsManager chatroomsManager,
            ILogger<WebSocketSession> wsSessionLogger)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (chatroomsManager is null)
            {
                throw new ArgumentNullException(nameof(chatroomsManager));
            }

            if (wsSessionLogger is null)
            {
                throw new ArgumentNullException(nameof(wsSessionLogger));
            }

            if (!httpContext.WebSockets.IsWebSocketRequest || httpContext.Request.Path != ChatClientEndpoint)
            {
                await _next(httpContext);
                return;
            }

            var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();

            using var webSocketSession = new WebSocketSession(webSocket, _messageParser, wsSessionLogger);

            var connectionRequest = webSocketSession.ReceivedMessages
                .Where(msg => msg is ConnectionRequestMessage)
                .Timeout(_connectionTimeout, Observable.Return(ClientMessageBase.Empty))
                .Take(1)
                .Wait() as ConnectionRequestMessage;

            if (connectionRequest is null)
            {
                _logger.LogDebug("Closing WebSockets connection due to authorization timeout.");
                await httpContext.Response.CompleteAsync();
                return;
            }

            httpContext.Request.Headers.Add(GetAuthorizationHeader(connectionRequest));
            var authResult = await httpContext.AuthenticateAsync(JwtSchemes.User);
            var subClaim = authResult.Principal?.FindFirst(JwtRegisteredClaimNames.Sub);
            if (!authResult.Succeeded ||
                subClaim is null ||
                !int.TryParse(subClaim.Value, out int idFromToken) ||
                connectionRequest.UserId != idFromToken)
            {
                _logger.LogDebug("Closing WebSockets connection due to authorization failure.");
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
