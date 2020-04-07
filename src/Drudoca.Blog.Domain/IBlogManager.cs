﻿using System;
using System.Threading.Tasks;

namespace Drudoca.Blog.Domain
{
    public interface IBlogManager
    {
        Task<BlogPage> GetPageAsync(int pageSize, int pageNum);
        Task<BlogPost?> GetPostAsync(DateTime published, string slug);
    }
}
