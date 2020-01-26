using Nvisibl.Cloud.WebSockets.Messages.Server.Base;
using System;

namespace Nvisibl.Cloud.WebSockets.Messages.Server
{
    [MessageInfo("CONNECTED")]
    public class ConnectedMessage : ServerMessageBase
    {
        public Guid SessionId { get; set; } = Guid.Empty;
    }
}
