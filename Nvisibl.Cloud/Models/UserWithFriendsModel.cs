using System.Collections.Generic;

namespace Nvisibl.Cloud.Models
{
    public class UserWithFriendsModel : UserModel
    {
        public IList<UserModel> Friends { get; set; } = new List<UserModel>();
    }
}
