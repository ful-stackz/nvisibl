using System;

namespace Nvisibl.Cloud.Models.Responses
{
    public class AuthLoginResponse
    {
        public AuthLoginResponse(int userId, AuthTokenResponse auth)
        {
            UserId = userId;
            Auth = auth ?? throw new ArgumentNullException(nameof(auth));
        }

        public int UserId { get; }
        public AuthTokenResponse Auth { get; }
    }
}
