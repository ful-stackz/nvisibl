using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models.Users
{
    public class UserModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;
    }
}
