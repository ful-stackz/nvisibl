using Nvisibl.Cloud.WebSockets.Messages.Server.Base;

namespace Nvisibl.Cloud.WebSockets.Messages.Server
{
    [MessageInfo("FRIEND_REQUEST")]
    public class FriendRequestMessage : ServerMessageBase
    {
        public int Id { get; set; }
        public bool Accepted { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; } = string.Empty;
        public int ReceiverId { get; set; }
        public string ReceiverUsername { get; set; } = string.Empty;
    }
}
