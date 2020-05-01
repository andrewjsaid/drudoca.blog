using System;
using System.Threading.Tasks;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess
{
    public interface ICommentRepository
    {
        Task CreateAsync(CommentData comment);
        Task<CommentData?> GetAsync(Guid id);
        Task<CommentData[]> GetByPostAsync(string postFileName);
        Task MarkDeletedAsync(Guid id);
    }
}
