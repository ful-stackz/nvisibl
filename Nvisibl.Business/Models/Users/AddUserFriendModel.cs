using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Business.Models.Users
{
    public class AddUserFriendModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int FriendId { get; set; }
    }
}
