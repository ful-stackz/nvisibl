using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Business.Models.Chatrooms
{
    public class ChatroomModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
