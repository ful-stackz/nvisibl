using Microsoft.Extensions.DependencyInjection;
using Nvisibl.Cloud.Services.Interfaces;
using Nvisibl.Cloud.WebSockets.Interfaces;
using Nvisibl.Cloud.WebSockets.Messages.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nvisibl.Cloud.WebSockets
{
    public class ClientManager : IChatClientManager, INotificationClientManager, IDisposable
    {
        private readonly object _lock = new object();
        private readonly IServiceProvider _serviceProvider;
        private readonly List<IClient> _clients = new List<IClient>();
        private readonly Dictionary<Guid, IDisposable> _subscriptions = new Dictionary<Guid, IDisposable>();

        private bool _isDisposed;

        public ClientManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void AddClient(IChatClient client)
        {
            lock (_lock)
            {
                if (!_clients.Any(c => c.UserId == client.UserId))
                {
                    _clients.Add(client);
                    _subscriptions.Add(
                        client.SessionId,
                        client.ReceivedMessages.Subscribe(async msg =>
                        {
                            using var serviceScope = _serviceProvider.CreateScope();
                            var messagesService = serviceScope.ServiceProvider.GetService<IMessagesManagerService>();
                            var chatroomService = serviceScope.ServiceProvider.GetService<IChatroomManagerService>();

                            var storedMessage = await messagesService.CreateMessageAsync(
                                Messages.Mappers.ToCreateMessageModel(msg));
                            var clientFriendlyMessage = new ChatroomMessageMessage
                            {
                                AuthorId = msg.AuthorId,
                                Body = storedMessage.Body,
                                ChatroomId = msg.ChatroomId,
                                MessageId = storedMessage.Id,
                                TimeSentUtc = msg.TimeSentUtc,
                            };
                            (await chatroomService.GetChatroomByIdWithUsersAsync(msg.ChatroomId))
                                .Users
                                .Where(user => user.Id != msg.AuthorId)
                                .Select(user => user.Id)
                                .ToList()
                                .ForEach(id =>
                                {
                                    GetClient<IChatClient>(c => c.UserId == id)?.SendMessage(clientFriendlyMessage);
                                });
                        }));
                }
            }
        }

        public void AddClient(INotificationClient client)
        {
            lock (_lock)
            {
                if (!_clients.Any(c => c.UserId == client.UserId))
                {
                    _clients.Add(client);
                }
            }
        }

        public bool RemoveClient(Func<IChatClient, bool> predicate)
        {
            lock (_lock)
            {
                return GetClient<IChatClient>(predicate) is { } client ? RemoveClient(client) : false;
            }
        }

        public bool RemoveClient(Func<INotificationClient, bool> predicate)
        {
            lock (_lock)
            {
                return GetClient<INotificationClient>(predicate) is { } client ? RemoveClient(client) : false;
            }
        }

        public IChatClient? GetClient(Func<IChatClient, bool> predicate)
        {
            lock (_lock)
            {
                return GetClient<IChatClient>(predicate);
            }
        }

        public INotificationClient? GetClient(Func<INotificationClient, bool> predicate)
        {
            lock (_lock)
            {
                 return GetClient<INotificationClient>(predicate);
            }
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
                    _subscriptions.Values.ToList().ForEach(s => s.Dispose());
                }
                _isDisposed = true;
            }
        }

        private T? GetClient<T>(Func<T, bool> predicate)
            where T : class, IClient =>
            _clients.Where(client => client is T).OfType<T>().FirstOrDefault(predicate);

        private bool RemoveClient(IClient client)
        {
            if (_subscriptions.TryGetValue(client.SessionId, out var subscription))
            {
                subscription.Dispose();
                _subscriptions.Remove(client.SessionId);
            }
            return _clients.Remove(client);
        }
    }
}
