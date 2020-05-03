﻿using System.Threading.Tasks;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess
{
    public interface ICommentRepository
    {
        Task<long> CreateAsync(CommentData comment);
        Task<CommentData?> GetAsync(long id);
        Task<CommentData[]> GetByPostAsync(string postFileName);
        Task MarkDeletedAsync(long id);
    }
}
