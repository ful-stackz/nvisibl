using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models.Requests
{
    public class AuthRenewTokenRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
    }
}
