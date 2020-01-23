using System.Collections.Generic;

namespace Nvisibl.Cloud.Models
{
    public class UserWithFriendsModel
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public IList<UserModel> Friends { get; set; } = new List<UserModel>();
    }
}
