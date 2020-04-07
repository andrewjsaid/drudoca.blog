using System.Threading.Tasks;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess.Store
{
    public interface IBlogStore
    {
        ValueTask<BlogPost[]> GetAllAsync();
    }
}