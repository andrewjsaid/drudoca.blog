using System.Data;

namespace Drudoca.Blog.DataAccess.Sql;

internal interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync();
}