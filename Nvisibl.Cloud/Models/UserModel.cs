using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models
{
    public class UserModel
    {
        [Required]
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;
    }
}
