namespace Nvisibl.Cloud.Models.Responses
{
    public class AuthTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date-time at which the token was created as an UTC string.
        /// </summary>
        public string CreatedAt { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date-time until which the token is valid as an UTC string.
        /// </summary>
        public string ValidBefore { get; set; }
    }
}
