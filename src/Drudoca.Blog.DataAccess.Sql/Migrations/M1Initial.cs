using System.Data.Common;
using System.Threading.Tasks;

namespace Drudoca.Blog.DataAccess.Sql.Migrations
{
    internal class M1Initial : IMigration
    {
        public async Task ApplyAsync(DbConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText = @"
                CREATE TABLE Comments (
	                Id              GUID NOT NULL,
	                PostFileName    TEXT(256) NOT NULL,
	                ParentId        GUID,
	                Author          TEXT(32) NOT NULL,
	                Email           TEXT(256) NOT NULL,
	                Markdown        TEXT(1024) NOT NULL,
	                PostedOnUtc     DATETIME NOT NULL,
	                IsDeleted       BOOLEAN DEFAULT 0 NOT NULL,

	                CONSTRAINT Comments_PK PRIMARY KEY (Id)
                );
                CREATE INDEX Comments_PostFileName_IDX ON Comments (PostFileName);
            ";

            await command.ExecuteNonQueryAsync();
        }
    }
}
