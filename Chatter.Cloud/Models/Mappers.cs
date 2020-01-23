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
    }
}
