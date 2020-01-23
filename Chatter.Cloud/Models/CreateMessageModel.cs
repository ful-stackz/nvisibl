using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models
{
    public class CreateMessageModel : MessageModel
    {
        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int ChatroomId { get; set; }
    }
}
