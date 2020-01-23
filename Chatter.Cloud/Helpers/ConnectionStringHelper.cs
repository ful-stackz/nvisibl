using Microsoft.Extensions.Configuration;
using System;

namespace Nvisibl.Cloud.Helpers
{
    internal static class ConnectionStringHelper
    {
        private const string ConnectionStringEnvVar = "DB_CONNECTIONSTRING";

        public static string GetConnectionString(IConfiguration configuration) =>
            configuration.GetValue(ConnectionStringEnvVar, string.Empty) is string connectionString &&
            !string.IsNullOrEmpty(connectionString)
            ? connectionString
            : throw new InvalidOperationException($"{ConnectionStringEnvVar} environment variable missing.");
    }
}
