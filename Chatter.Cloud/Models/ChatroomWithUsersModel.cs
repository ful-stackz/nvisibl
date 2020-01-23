using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models
{
    public class ChatroomWithUsersModel : ChatroomModel
    {
        public IList<UserModel> Users { get; set; } = new List<UserModel>();
    }
}
