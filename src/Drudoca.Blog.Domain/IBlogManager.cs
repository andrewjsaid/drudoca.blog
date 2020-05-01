using System;
using System.Threading.Tasks;

namespace Drudoca.Blog.Domain
{
    public interface IBlogManager
    {
        Task<BlogPage> GetPageAsync(int pageSize, int pageNum);
        Task<BlogPost?> GetPostAsync(DateTime published, string slug);
        Task<BlogComment[]> GetCommentsAsync(string postFileName);
        Task CreateCommentAsync(string postFileName, Guid? parentId, string author, string email, string markdown);
        Task DeleteCommentAsync(Guid id);
    }
}
