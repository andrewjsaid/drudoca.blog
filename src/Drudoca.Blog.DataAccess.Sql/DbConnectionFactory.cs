using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using Drudoca.Blog.Config;

namespace Drudoca.Blog.DataAccess.Sql
{
    internal class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly MigrationManager _migrationManager;
        private readonly DatabaseOptions _options;

        public DbConnectionFactory(
            MigrationManager migrationManager,
            DatabaseOptions options)
        {
            _migrationManager = migrationManager;
            _options = options;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connectionString = _options.SqliteConnectionString;
            var connection = new SQLiteConnection(connectionString);
            
            await connection.OpenAsync();
            
            await _migrationManager.RunAsync(connection);

            return connection;
        }
    }
}
