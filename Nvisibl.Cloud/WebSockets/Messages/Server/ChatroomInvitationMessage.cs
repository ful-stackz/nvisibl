using Nvisibl.Cloud.WebSockets.Messages.Server.Base;
using System;
using System.Collections.Generic;

namespace Nvisibl.Cloud.WebSockets.Messages.Server
{
    [MessageInfo("CHATROOM_INVITATION")]
    public class ChatroomInvitationMessage : ServerMessageBase
    {
        public int ChatroomId { get; set; }
        public string ChatroomName { get; set; } = string.Empty;
        public IList<int> Users { get; set; } = Array.Empty<int>();
    }
}
