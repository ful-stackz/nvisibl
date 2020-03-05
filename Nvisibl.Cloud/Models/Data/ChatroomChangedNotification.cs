using Nvisibl.Business.Models.Chatrooms;
using System;

namespace Nvisibl.Cloud.Models.Data
{
    internal class ChatroomChangedNotification : Notification
    {
        public ChatroomChangedNotification(ChatroomModel chatroom)
        {
            Chatroom = chatroom ?? throw new ArgumentNullException(nameof(chatroom));
        }

        public ChatroomModel Chatroom { get; }
    }
}
