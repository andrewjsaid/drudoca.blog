using System;
using System.Threading.Tasks;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess
{

    public interface IBlogRepository
    {

        ValueTask<BlogPost?> GetPostAsync(DateTime published, string slug);

        ValueTask<BlogPage> GetPageAsync(int pageSize, int pageNum);

    }
}
