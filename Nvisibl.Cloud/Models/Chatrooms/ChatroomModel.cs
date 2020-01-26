using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models.Chatrooms
{
    public class ChatroomModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
