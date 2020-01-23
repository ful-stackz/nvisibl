using Nvisibl.Cloud.WebSockets.Messages.Server;

namespace Nvisibl.Cloud.WebSockets.Interfaces
{
    public interface INotificationClient : IClient
    {
        void SendFriendRequest(FriendRequestMessage friendRequest);
        void SendChatroomInvite(ChatroomInvitationMessage chatroomInvitation);
    }
}
