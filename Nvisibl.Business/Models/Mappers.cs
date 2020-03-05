using Nvisibl.Business.Models.Chatrooms;
using Nvisibl.Business.Models.Messages;
using Nvisibl.Business.Models.Users;
using Nvisibl.DataLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nvisibl.Business.Models
{
    internal static class Mappers
    {
        internal static ChatroomModel ToChatroomModel(Chatroom chatroom, IEnumerable<User> users) =>
            new ChatroomModel
            {
                Id = chatroom.Id,
                Name = chatroom.Name,
                IsShared = chatroom.IsShared,
                Users = users?.Select(ToUserModel).ToList() ?? Enumerable.Empty<UserModel>().ToList(),
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
