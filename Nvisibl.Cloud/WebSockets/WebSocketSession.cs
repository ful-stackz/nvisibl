using Microsoft.Extensions.Logging;
using Nvisibl.Cloud.WebSockets.Messages.Client.Base;
using Nvisibl.Cloud.WebSockets.Messages.Interfaces;
using Nvisibl.Cloud.WebSockets.Messages.Server.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.WebSockets
{
    public class WebSocketSession : IDisposable
    {
        private readonly Thread _incomingMessagesProcessor;
        private readonly Thread _outgoingMessagesProcessor;
        private readonly Subject<ClientMessageBase> _receivedMessagesSubject = new Subject<ClientMessageBase>();
        private readonly ConcurrentQueue<ServerMessageBase> _outgoing = new ConcurrentQueue<ServerMessageBase>();

        private bool _isDisposed;

        public WebSocketSession(WebSocket webSocket, IMessageParser messageParser, ILogger<WebSocketSession> logger)
        {
            if (webSocket is null)
            {
                throw new ArgumentNullException(nameof(webSocket));
            }

            if (messageParser is null)
            {
                throw new ArgumentNullException(nameof(messageParser));
            }

            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _incomingMessagesProcessor = new Thread(() =>
            {
                try
                {
                    var received = new List<byte[]>(4);
                    byte[] buffer = new byte[1024 * 4];
                    while (!_isDisposed && !webSocket.CloseStatus.HasValue && webSocket.State == WebSocketState.Open)
                    {
                        var result = webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None)
                            .GetAwaiter()
                            .GetResult();
                        received.Add(buffer[0..result.Count]);
                        if (result.EndOfMessage)
                        {
                            var messageRaw = Encoding.UTF8.GetString(received.SelectMany(x => x).ToArray());
                            var message = messageParser.DeserializeClientMessage(messageRaw);
                            if (!_receivedMessagesSubject.IsDisposed)
                            {
                                _receivedMessagesSubject.OnNext(message);
                            }
                            received.Clear();
                        }
                        Array.Clear(buffer, 0, buffer.Length);
                    }
                }
                catch (WebSocketException ex)
                {
                    logger.LogWarning(ex, $"WebSockets session terminated. Status: {ex.WebSocketErrorCode}.");
                }
                finally
                {
                    _ = Lifetime.TrySetResult(new object());
                }
            });
            _outgoingMessagesProcessor = new Thread(() =>
            {
                while (!_isDisposed && !webSocket.CloseStatus.HasValue && webSocket.State == WebSocketState.Open)
                {
                    if (!_outgoing.TryDequeue(out var message))
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    var messageRaw = messageParser.SerializeServerMessage(message);

                    try
                    {
                        webSocket.SendAsync(new ArraySegment<byte>(
                            Encoding.UTF8.GetBytes(messageRaw)),
                            WebSocketMessageType.Text,
                            endOfMessage: true,
                            CancellationToken.None)
                        .GetAwaiter()
                        .GetResult();
                    }
                    catch (WebSocketException ex)
                    {
                        logger.LogWarning(
                            exception: ex,
                            $"Could not send message over WebSockets. Status: {ex.WebSocketErrorCode}.");
                    }
                }
                _ = Lifetime.TrySetResult(new object());
            });
            _incomingMessagesProcessor.Start();
            _outgoingMessagesProcessor.Start();
        }

        public TaskCompletionSource<object> Lifetime { get; } = new TaskCompletionSource<object>();

        public IObservable<ClientMessageBase> ReceivedMessages =>
            _receivedMessagesSubject.AsObservable();

        public void EnqueueMessage(ServerMessageBase message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            _outgoing.Enqueue(message);
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
                    _receivedMessagesSubject.Dispose();
                }
                _isDisposed = true;
            }
        }
    }
}
