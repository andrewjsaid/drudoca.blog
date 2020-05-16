using System.Data;
using System.Threading.Tasks;

namespace Drudoca.Blog.DataAccess.Sql
{
    internal interface IDbConnectionFactory
    {
        public Task<IDbConnection> CreateConnectionAsync();
    }
}
