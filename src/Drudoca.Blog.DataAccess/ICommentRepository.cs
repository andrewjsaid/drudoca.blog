using System.Threading.Tasks;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess
{
    public interface ICommentRepository
    {
        Task CreateAsync(CommentData comment);
        Task<CommentData[]> GetCommentsForPostAsync(string postFileName);
    }
}
