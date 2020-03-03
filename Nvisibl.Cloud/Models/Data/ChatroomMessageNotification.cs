using Nvisibl.Business.Models.Messages;
using System;

namespace Nvisibl.Cloud.Models.Data
{
    public class ChatroomMessageNotification : Notification
    {
        public ChatroomMessageNotification(MessageModel message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public MessageModel Message { get; }
    }
}
