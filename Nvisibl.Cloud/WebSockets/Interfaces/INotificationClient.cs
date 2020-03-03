using System;

namespace Nvisibl.Cloud.WebSockets.Interfaces
{
    public interface INotificationClient
    {
        public int UserId { get; }
        public Guid SessionId { get; }
    }
}
