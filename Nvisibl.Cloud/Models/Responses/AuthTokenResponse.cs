using System;

namespace Nvisibl.Cloud.Models.Responses
{
    public class AuthTokenResponse
    {
        public AuthTokenResponse(string accessToken, string createdAt, string validBefore)
        {
            AccessToken = string.IsNullOrEmpty(accessToken)
                ? throw new ArgumentException("Argument was null or empty.", nameof(accessToken))
                : accessToken;
            CreatedAt = string.IsNullOrEmpty(createdAt)
                ? throw new ArgumentException("Argument was null or empty.", nameof(createdAt))
                : createdAt;
            ValidBefore = string.IsNullOrEmpty(validBefore)
                ? throw new ArgumentException("Argument was null or empty.", nameof(validBefore))
                : validBefore;
        }

        public string AccessToken { get; }

        /// <summary>
        /// Gets or sets the date-time at which the token was created as an UTC string.
        /// </summary>
        public string CreatedAt { get; }

        /// <summary>
        /// Gets or sets the date-time until which the token is valid as an UTC string.
        /// </summary>
        public string ValidBefore { get; }
    }
}
