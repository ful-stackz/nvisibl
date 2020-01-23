using Nvisibl.Cloud.WebSockets.Messages.Client.Base;

namespace Nvisibl.Cloud.WebSockets.Messages.Client
{
    [MessageInfo("CONNECTION_REQUEST")]
    public class ConnectionRequestMessage : ClientMessageBase
    {
        public int UserId { get; set; }
    }
}
