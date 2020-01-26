using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Business.Models.Chatrooms
{
    public class AddUserToChatroomModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ChatroomId { get; set; }
    }
}
