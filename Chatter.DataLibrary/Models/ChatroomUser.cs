using System.ComponentModel.DataAnnotations;

namespace Nvisibl.DataLibrary.Models
{
    public class ChatroomUser
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ChatroomId { get; set; }

        public User? User { get; set; }

        public Chatroom? Chatroom { get; set; }
    }
}
