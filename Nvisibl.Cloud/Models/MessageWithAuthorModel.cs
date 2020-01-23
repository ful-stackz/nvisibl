using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models
{
    public class MessageWithAuthorModel : MessageModel
    {
        [Required]
        public UserModel Author { get; set; } = new UserModel();
    }
}
