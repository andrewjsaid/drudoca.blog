using System;
using System.Threading.Tasks;

namespace Drudoca.Blog.Domain
{
    public interface IBlogService
    {
        Task<BlogPage> GetPageAsync(int pageSize, int pageNum);
        Task<BlogPost?> GetPostAsync(DateTime published, string slug);

        Task<int> CountCommentsAsync(string postFileName);
        Task<BlogComment[]> GetCommentsAsync(string postFileName);
        Task<long?> CreateCommentAsync(string postFileName, long? parentId, string author, string email, string markdown);
        Task DeleteCommentAsync(long id);
    }
}
