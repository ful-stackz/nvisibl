using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models
{
    public class ChatroomModel
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
