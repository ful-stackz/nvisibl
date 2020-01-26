using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models.Chatrooms
{
    public class AddUserToChatroomModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ChatroomId { get; set; }
    }
}
