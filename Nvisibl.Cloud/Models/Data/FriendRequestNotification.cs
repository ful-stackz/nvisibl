using Nvisibl.Business.Models.Users;
using System;

namespace Nvisibl.Cloud.Models.Data
{
    internal class FriendRequestNotification : Notification
    {
        public FriendRequestNotification(FriendModel friendRequest, UserModel sender, UserModel receiver)
        {
            FriendRequest = friendRequest ?? throw new ArgumentNullException(nameof(friendRequest));
            Sender = sender ?? throw new ArgumentNullException(nameof(sender));
            Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        }

        public FriendModel FriendRequest { get; }
        public UserModel Sender { get; }
        public UserModel Receiver { get; }
    }
}
