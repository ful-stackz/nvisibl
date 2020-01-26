namespace Nvisibl.Cloud.WebSockets.Messages.Client.Base
{
    public abstract class ClientMessageBase
    {
        public static ClientMessageBase Empty { get; } = new EmptyClientMessage();
    }
}
