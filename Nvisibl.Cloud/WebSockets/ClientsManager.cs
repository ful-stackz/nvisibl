using Nvisibl.Cloud.WebSockets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nvisibl.Cloud.WebSockets
{
    public class ClientsManager : IChatClientsManager, INotificationClientManager
    {
        private readonly object _lock = new object();
        private readonly List<IClient> _clients = new List<IClient>();

        public void AddClient(IChatClient client)
        {
            lock (_lock)
            {
                if (!_clients.Any(c => c.UserId == client.UserId))
                {
                    _clients.Add(client);
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
                return GetClient<IChatClient>(predicate) is { } client ? _clients.Remove(client) : false;
            }
        }

        public bool RemoveClient(Func<INotificationClient, bool> predicate)
        {
            lock (_lock)
            {
                return GetClient<INotificationClient>(predicate) is { } client ? _clients.Remove(client) : false;
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

        private T? GetClient<T>(Func<T, bool> predicate)
            where T : class, IClient =>
            _clients.Where(client => client is T).OfType<T>().FirstOrDefault(predicate);
    }
}
