using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Business.Models.Chatrooms
{
    public class CreateChatroomModel
    {
        [Required]
        public int OwnerId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string ChatroomName { get; set; } = string.Empty;

        [Required]
        public bool IsShared { get; set; }
    }
}
