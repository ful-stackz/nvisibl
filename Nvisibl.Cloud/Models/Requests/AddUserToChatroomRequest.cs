using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models.Requests
{
    public class AddUserToChatroomRequest
    {
        [Required]
        public int UserId { get; set; }
    }
}
