using Nvisibl.Cloud.WebSockets.Messages.Server.Base;

namespace Nvisibl.Cloud.WebSockets.Messages.Server
{
    [MessageInfo("FRIEND_REQUEST")]
    public class FriendRequestMessage : ServerMessageBase
    {
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
    }
}
