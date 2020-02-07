using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Business.Models.Chatrooms
{
    public class CreateChatroomModel
    {
        [Required]
        public int OwnerId { get; set; }

        [MinLength(3)]
        [MaxLength(100)]
        public string? ChatroomName { get; set; } = null;

        [Required]
        public bool IsShared { get; set; }
    }
}
