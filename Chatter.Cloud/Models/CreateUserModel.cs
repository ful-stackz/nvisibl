using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models
{
    public class CreateUserModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
    }
}
