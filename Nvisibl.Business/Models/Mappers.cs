using Nvisibl.Business.Models.Chatrooms;
using Nvisibl.Business.Models.Messages;
using Nvisibl.Business.Models.Users;
using Nvisibl.DataLibrary.Models;

namespace Nvisibl.Business.Models
{
    internal static class Mappers
    {
        internal static ChatroomModel ToChatroomModel(Chatroom chatroom) =>
            new ChatroomModel
            {
                Id = chatroom.Id,
                Name = chatroom.Name,
            };

        internal static MessageModel ToMessageModel(Message message) =>
            new MessageModel
            {
                AuthorId = message.AuthorId,
                Body = message.Body,
                ChatroomId = message.ChatroomId,
                Id = message.Id,
                TimeSentUtc = message.TimeSentUtc,
            };

        internal static UserModel ToUserModel(User user) =>
            new UserModel
            {
                Id = user.Id,
                Username = user.Username,
            };
    }
}
