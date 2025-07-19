using System.Data;
using System.Data.SQLite;
using Microsoft.Extensions.Options;

namespace Drudoca.Blog.DataAccess.Sql;

internal class DbConnectionFactory(
    MigrationManager migrationManager,
    IOptions<DatabaseOptions> options)
    : IDbConnectionFactory
{
    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connectionString = options.Value.SqliteConnectionString;
        var connection = new SQLiteConnection(connectionString);
            
        await connection.OpenAsync();
            
        await migrationManager.RunAsync(connection);

        return connection;
    }
}