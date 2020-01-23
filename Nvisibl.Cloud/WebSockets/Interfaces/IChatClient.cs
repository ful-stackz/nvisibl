using System;

namespace Nvisibl.Cloud.WebSockets.Interfaces
{
    public interface IChatClient : IClient
    {
        IObservable<Messages.Client.ChatroomMessageMessage> ReceivedMessages { get; }

        void SendMessage(Messages.Server.ChatroomMessageMessage message);
    }
}
