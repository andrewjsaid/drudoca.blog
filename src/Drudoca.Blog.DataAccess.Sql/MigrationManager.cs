using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Drudoca.Blog.DataAccess.Sql.Migrations;
using Microsoft.Extensions.Logging;
using Dapper;
using System.Diagnostics;
using System.Linq;

namespace Drudoca.Blog.DataAccess.Sql
{
    internal class MigrationManager
    {
        private static IMigration[] Migrations { get; } = {
            new M1Initial()
        };

        private static string? _lastConnectionString = null;

        private readonly ILogger<MigrationManager> _logger;

        public MigrationManager(ILogger<MigrationManager> logger)
        {
            _logger = logger;
        }

        public Task RunAsync(DbConnection connection)
        {
            // By far the most common use case is for there to be a single connection string
            // reused over and over. No need to run the migration each time.
            // Also avoids creating GC pressure with unnecessary Tasks.
            if (connection.ConnectionString == _lastConnectionString)
                return Task.CompletedTask;

            // TODO AJS: Add some form of lock over here as we don't
            // want multiple migrations to run at the same time.
            return RunAsyncInternal(connection);
        }

        private async Task RunAsyncInternal(DbConnection connection)
        {
            await EnsureMigrationTableExistsAsync(connection);
            var migrationHistory = await GetMigrationHistoryAsync(connection);

            foreach (var migration in Migrations)
            {
                var name = migration.GetType().Name;
                if (Array.IndexOf(migrationHistory, name) == -1)
                {
                    _logger.LogDebug("Applying migration {name}", name);
                    await migration.ApplyAsync(connection);
                    await AddMigrationAsync(connection, name);
                    _logger.LogDebug("Applied migration {name}", name);
                }
            }

            _lastConnectionString = connection.ConnectionString;
        }

        private async Task EnsureMigrationTableExistsAsync(DbConnection connection)
        {
            const string sql = "SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='_MigrationHistory';";
            var count = await connection.QueryFirstAsync<int>(sql);
            Debug.Assert(count == 0 || count == 1);

            if (count == 1)
                return;

            _logger.LogDebug("_MigrationHistory table does not exist - creating it.");

            const string createMigrationTableSql = @"
                CREATE TABLE ""_MigrationHistory"" (
                    Name TEXT(256) NOT NULL,
                    AppliedAtUtc TEXT(28) NOT NULL
                );
            ";
            await connection.ExecuteAsync(createMigrationTableSql);

            _logger.LogDebug("Created _MigrationHistory table.");
        }

        private async Task<string[]> GetMigrationHistoryAsync(DbConnection connection)
        {
            const string sql = "SELECT Name FROM _MigrationHistory ORDER BY AppliedAtUtc";
            var query = await connection.QueryAsync<string>(sql);
            return query.ToArray();
        }

        private async Task AddMigrationAsync(DbConnection connection, string name)
        {
            const string sql = @"
                INSERT INTO _MigrationHistory (Name, AppliedAtUtc)
                VALUES (:name, :appliedAtUtc)
            ";

            await connection.ExecuteAsync(sql, new
            {
                name = name,
                appliedAtUtc = DateTime.UtcNow
            });
        }
    }
}
