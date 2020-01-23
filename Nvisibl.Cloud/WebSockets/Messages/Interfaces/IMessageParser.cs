using Nvisibl.Cloud.WebSockets.Messages.Client.Base;
using Nvisibl.Cloud.WebSockets.Messages.Server.Base;

namespace Nvisibl.Cloud.WebSockets.Messages.Interfaces
{
    public interface IMessageParser
    {
        public string SerializeServerMessage(ServerMessageBase message);
        public ClientMessageBase DeserializeClientMessage(string message);
    }
}
