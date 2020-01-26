﻿using Nvisibl.Cloud.WebSockets.Messages.Server.Base;
using System;

namespace Nvisibl.Cloud.WebSockets.Messages.Server
{
    [MessageInfo("CHAT_MESSAGE_RECEIVE")]
    public sealed class ChatroomMessageMessage : ServerMessageBase
    {
        public int AuthorId { get; set; }

        public int ChatroomId { get; set; }

        public int MessageId { get; set; }

        public string Body { get; set; } = string.Empty;

        public DateTime TimeSentUtc { get; set; }
    }
}
