using System;

namespace Nvisibl.Cloud.WebSockets.Interfaces
{
    public interface IClientManager<T>
        where T : class, IClient
    {
        void AddClient(T client);
        bool RemoveClient(Func<T, bool> predicate);
        T? GetClient(Func<T, bool> predicate);
    }
}
