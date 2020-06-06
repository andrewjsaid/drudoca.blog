using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess
{
    public interface IPostRepository
    {
        Task<int> CountPostsAsync();
        IAsyncEnumerable<PostData> GetAllPostsAsync();
        Task<PostData?> GetPostByFileName(string fileName);
        IAsyncEnumerable<PostData> GetPostsByDateAsync(DateTime published);
    }
}
