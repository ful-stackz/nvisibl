using System;
using System.Collections.Generic;
using System.Linq;

namespace Nvisibl.Cloud.Models.Data
{
    internal class ChatroomChangedNotification : Notification
    {
        public ChatroomChangedNotification(int chatroomId, string chatroomName, IEnumerable<int> participants)
        {
            ChatroomId = chatroomId;
            ChatroomName = chatroomName;
            Participants = participants?.ToList() ?? throw new ArgumentNullException(nameof(participants));
        }

        public int ChatroomId { get; }
        public string ChatroomName { get; }
        public IList<int> Participants { get; }
    }
}
