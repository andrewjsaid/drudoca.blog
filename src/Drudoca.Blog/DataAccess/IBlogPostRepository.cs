using Drudoca.Blog.Models;
using System.Threading.Tasks;

namespace Drudoca.Blog.DataAccess
{
    public interface IBlogPostRepository
    {

        ValueTask<BlogPost> GetBlogPost(string slug);

        ValueTask<BlogPosts> GetBlogPostsAsync(int pageNum);

    }
}
