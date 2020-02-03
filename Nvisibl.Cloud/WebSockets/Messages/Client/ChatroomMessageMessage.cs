﻿using Nvisibl.Cloud.WebSockets.Messages.Client.Base;

namespace Nvisibl.Cloud.WebSockets.Messages.Client
{
    [MessageInfo("CHAT_MESSAGE_SEND")]
    public sealed class ChatroomMessageMessage : ClientMessageBase
    {
        public int AuthorId { get; set; }

        public int ChatroomId { get; set; }

        public string Body { get; set; } = string.Empty;

        public string TimeSentUtc { get; set; } = string.Empty;
    }
}
