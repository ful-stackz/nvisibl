using System;

namespace Nvisibl.Cloud.WebSockets.Interfaces
{
    public interface IClient
    {
        public int UserId { get; }
        public Guid SessionId { get; }
    }
}
