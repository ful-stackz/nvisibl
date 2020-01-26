using Nvisibl.Cloud.WebSockets.Messages.Server.Base;

namespace Nvisibl.Cloud.WebSockets.Messages.Server
{
    [MessageInfo("CHATROOM_INVITATION")]
    public class ChatroomInvitationMessage : ServerMessageBase
    {
        public int ChatroomId { get; set; }
        public string ChatroomName { get; set; } = string.Empty;
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
    }
}
