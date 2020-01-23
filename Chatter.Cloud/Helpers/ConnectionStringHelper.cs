using Microsoft.Extensions.Configuration;
using System;

namespace Nvisibl.Cloud.Helpers
{
    internal static class ConnectionStringHelper
    {
        private const string DBDataSourceEnvVar = "DB_DATASOURCE";
        private const string DBDatabaseEnvVar = "DB_DATABASE";

        public static string GetConnectionString(IConfiguration configuration)
        {
            string dataSource = configuration.GetValue(DBDataSourceEnvVar, string.Empty);
            if (string.IsNullOrEmpty(dataSource))
            {
                throw new InvalidOperationException($"{DBDataSourceEnvVar} environment variable missing.");
            }

            string database = configuration.GetValue(DBDatabaseEnvVar, string.Empty);
            if (string.IsNullOrEmpty(database))
            {
                throw new InvalidOperationException($"{DBDatabaseEnvVar} environment variable missing.");
            }

            return $"Data Source={dataSource};Initial Catalog={database};Integrated Security=True";
        }
    }
}
