using System.Data.Common;

namespace Drudoca.Blog.DataAccess.Sql;

internal interface IMigration
{
    Task ApplyAsync(DbConnection connection);
}