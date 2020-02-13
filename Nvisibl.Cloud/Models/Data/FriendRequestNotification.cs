using Nvisibl.Business.Models.Users;
using System;

namespace Nvisibl.Cloud.Models.Data
{
    internal class FriendRequestNotification : Notification
    {
        public FriendRequestNotification(int friendRequestId, bool accepted, UserModel sender, UserModel receiver)
        {
            FriendRequestId = friendRequestId;
            Accepted = accepted;
            Sender = sender ?? throw new ArgumentNullException(nameof(sender));
            Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        }

        public int FriendRequestId { get; }
        public bool Accepted { get; }
        public UserModel Sender { get; }
        public UserModel Receiver { get; }
    }
}
