using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models.Requests
{
    public class CreateChatroomRequest
    {
        [Required]
        public int OwnerId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string ChatroomName { get; set; } = string.Empty;

        public bool IsShared { get; set; } = false;
    }
}
