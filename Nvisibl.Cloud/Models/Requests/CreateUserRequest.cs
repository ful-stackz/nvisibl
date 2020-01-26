using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models.Requests
{
    public class CreateUserRequest
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
    }
}
