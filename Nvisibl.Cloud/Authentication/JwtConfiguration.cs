using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Nvisibl.Cloud.Authentication
{
    public class JwtConfiguration
    {
        private const string SchemePostfix = "AuthConfig";
        private const string IssuerKey = "Issuer";
        private const string AudienceKey = "Audience";
        private const string SecretKey = "Secret";

        private readonly IConfiguration _configuration;

        internal static string SecurityAlgorithm { get; } = SecurityAlgorithms.HmacSha256;

        internal JwtConfiguration(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        internal JwtSchemeConfig GetSchemeConfig(string scheme)
        {
            var configSection = _configuration.GetSection($"{scheme}{SchemePostfix}");
            if (!configSection.Exists())
            {
                throw new InvalidOperationException($"Scheme '{scheme}' does not exist.");
            }

            string issuer = configSection.GetValue(IssuerKey, string.Empty);
            string audience = configSection.GetValue(AudienceKey, string.Empty);
            string secret = configSection.GetValue(SecretKey, string.Empty);

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(secret))
            {
                throw new InvalidOperationException($"Scheme '{scheme}' configuration is incomplete or invalid.");
            }

            return new JwtSchemeConfig(issuer, audience, GetSecurityKey(secret));
        }

        internal void ConfigureJwtBearerOptions(string scheme, JwtBearerOptions options)
        {
            if (string.IsNullOrEmpty(scheme))
            {
                throw new ArgumentException("Argument was null or empty.", nameof(scheme));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var config = GetSchemeConfig(scheme);
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = config.Issuer,
                ValidAudience = config.Audience,
                IssuerSigningKey = config.SecurityKey,
            };
        }

        private SecurityKey GetSecurityKey(string secret) =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    }
}
