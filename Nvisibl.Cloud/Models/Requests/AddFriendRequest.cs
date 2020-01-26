using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models.Requests
{
    public class AddFriendRequest
    {
        [Required]
        public int UserId { get; set; }
    }
}
