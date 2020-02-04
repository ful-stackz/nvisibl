namespace Nvisibl.Cloud.Models.Responses
{
    public class AuthLoginResponse
    {
        public int UserId { get; set; }
        public string AccessToken { get; set; } = string.Empty;
    }
}
