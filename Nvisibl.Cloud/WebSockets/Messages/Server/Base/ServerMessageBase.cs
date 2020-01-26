namespace Nvisibl.Cloud.WebSockets.Messages.Server.Base
{
    public abstract class ServerMessageBase
    {
        public static ServerMessageBase Empty { get; } = new EmptyServerMessage();
    }
}
