using Drudoca.Blog.Models;
using System;

namespace Drudoca.Blog.DataAccess
{
    public class CachedBlogPosts
    {
        public CachedBlogPosts(BlogPost[] blogPosts, DateTime expiry)
        {
            BlogPosts = blogPosts ?? throw new ArgumentNullException(nameof(blogPosts));
            Expiry = expiry;
        }
        public BlogPost[] BlogPosts { get; }
        public DateTime Expiry { get; }
    }
}