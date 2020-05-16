using System.Data.Common;
using System.Threading.Tasks;

namespace Drudoca.Blog.DataAccess.Sql
{
    internal interface IMigration
    {
        Task ApplyAsync(DbConnection connection);
    }
}
