using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models.Requests
{
    public class CreateChatroomRequest
    {
        [Required]
        public int OwnerId { get; set; }

        [MinLength(3)]
        [MaxLength(100)]
        public string? ChatroomName { get; set; } = null;

        public bool IsShared { get; set; } = false;
    }
}
