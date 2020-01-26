using Nvisibl.Business.Models.Messages;

namespace Nvisibl.Cloud.WebSockets.Interfaces
{
    public interface IChatClient : IClient
    {
        void SendMessage(MessageModel message);
    }
}
