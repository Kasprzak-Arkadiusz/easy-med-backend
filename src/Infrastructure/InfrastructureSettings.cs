#pragma warning disable CS8603
using Npgsql;

namespace EasyMed.Infrastructure;

public class InfrastructureSettings
{
    private string _dbConnectionString;

    public string DbConnectionString
    {
        get
        {
            var databaseUrl = Environment.GetEnvironmentVariable(_dbConnectionString ?? "");
            return databaseUrl != null ? ParseDatabaseUrl(databaseUrl) : _dbConnectionString;
        }

        set => _dbConnectionString = value;
    }

    private static string ParseDatabaseUrl(string databaseUrl)
    {
        var databaseUri = new Uri(databaseUrl);
        var userInfo = databaseUri.UserInfo.Split(':');

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port,
            Username = userInfo[0],
            Password = userInfo[1],
            Database = databaseUri.LocalPath.TrimStart('/'),
            Pooling = true,
            SslMode = SslMode.Require,
            TrustServerCertificate = true
        };

        return builder.ToString();
    }
}