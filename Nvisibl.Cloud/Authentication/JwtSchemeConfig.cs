using Microsoft.IdentityModel.Tokens;
using System;

namespace Nvisibl.Cloud.Authentication
{
    internal class JwtSchemeConfig
    {
        internal JwtSchemeConfig(string issuer, string audience, SecurityKey securityKey)
        {
            Issuer = string.IsNullOrEmpty(issuer)
                ? throw new ArgumentException("Argument was null or empty.", nameof(issuer))
                : issuer;
            Audience = string.IsNullOrEmpty(audience)
                ? throw new ArgumentException("Argument was null or empty.", nameof(audience))
                : audience;
            SecurityKey = securityKey ?? throw new ArgumentNullException(nameof(securityKey));
        }

        internal string Issuer { get; }
        internal string Audience { get; }
        internal SecurityKey SecurityKey { get; }
    }
}
