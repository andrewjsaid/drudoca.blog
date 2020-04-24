using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Microsoft.Extensions.Options;

namespace Drudoca.Blog.DataAccess.Sql
{
    internal class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly MigrationManager _migrationManager;
        private readonly IOptions<DatabaseOptions> _options;

        public DbConnectionFactory(
            MigrationManager migrationManager,
            IOptions<DatabaseOptions> options)
        {
            _migrationManager = migrationManager;
            _options = options;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connectionString = _options.Value.SqliteConnectionString;
            var connection = new SQLiteConnection(connectionString);
            
            await connection.OpenAsync();
            
            await _migrationManager.RunAsync(connection);

            return connection;
        }
    }
}
