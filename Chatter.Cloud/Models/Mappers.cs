using Nvisibl.DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nvisibl.Cloud.Models
{
    internal static class Mappers
    {
        internal static UserModel ToUserModel(User user) =>
            user is null
            ? throw new ArgumentNullException(nameof(user))
            : new UserModel
            {
                Id = user.Id,
                Username = user.Username,
            };

        internal static UserWithFriendsModel ToUserWithFriendsModel(User user, IEnumerable<User> friends)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (friends is null)
            {
                throw new ArgumentNullException(nameof(friends));
            }

            return new UserWithFriendsModel
            {
                Id = user.Id,
                Username = user.Username,
                Friends = friends.Select(ToUserModel).ToList(),
            };
        }

        internal static ChatroomModel ToChatroomModel(Chatroom chatroom) =>
            chatroom is null
            ? throw new ArgumentNullException(nameof(chatroom))
            : new ChatroomModel
            {
                Id = chatroom.Id,
                Name = chatroom.Name,
            };

        internal static ChatroomWithUsersModel ToChatroomWithUsersModel(Chatroom chatroom, IEnumerable<User> users)
        {
            if (chatroom is null)
            {
                throw new ArgumentNullException(nameof(chatroom));
            }

            if (users is null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            return new ChatroomWithUsersModel
            {
                Id = chatroom.Id,
                Name = chatroom.Name,
                Users = users.Select(ToUserModel).ToList(),
            };
        }
    }
}
