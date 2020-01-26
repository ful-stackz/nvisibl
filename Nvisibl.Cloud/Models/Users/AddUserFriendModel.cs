using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models.Users
{
    public class AddUserFriendModel
    {
        [Required]
        public int UserId { get; set; }
    }
}
