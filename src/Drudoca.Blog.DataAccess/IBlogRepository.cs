using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess
{
    public interface IBlogRepository
    {
        Task<int> CountBlogPostsAsync();
        IAsyncEnumerable<BlogPostData> GetAllPostsAsync();
        IAsyncEnumerable<BlogPostData> GetPostsByDateAsync(DateTime published);
    }
}
