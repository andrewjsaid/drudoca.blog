using System;
using System.Threading.Tasks;
using Drudoca.Blog.Data;
using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.DataAccess.Store
{
    public sealed class StoreBlogRepository : IBlogRepository
    {
        private readonly IBlogStore _store;
        private readonly ILogger _logger;

        public StoreBlogRepository(
            IBlogStore store,
            ILogger<StoreBlogRepository> logger)
        {
            _store = store;
            _logger = logger;
        }

        public async ValueTask<BlogPage> GetPageAsync(int pageSize, int pageNum)
        {
            if (pageNum <= 0) throw new ArgumentOutOfRangeException(nameof(pageNum), "pageNum must not be less than 0");

            _logger.LogDebug("Retrieving {page-size} posts on page {page-num}", pageSize, pageNum);

            int skip = pageSize * (pageNum - 1);

            var allPosts = await _store.GetAllAsync();

            var pagePosts = new BlogPost[Math.Min(pageSize, allPosts.Length - skip)];
            Array.Copy(allPosts, skip, pagePosts, 0, pagePosts.Length);

            var pageCount = (int)Math.Ceiling(allPosts.Length / (double)pageSize);

            var result = new BlogPage(pageNum, pageSize, pageCount, pagePosts);
            return result;
        }

        public async ValueTask<BlogPost?> GetPostAsync(DateTime published, string slug)
        {
            _logger.LogDebug("Retrieving post with slug {slug}", slug);

            var allPosts = await _store.GetAllAsync();

            foreach (var post in allPosts)
            {
                if (post.PublishedOn.Date == published && string.Equals(post.Slug, slug, StringComparison.OrdinalIgnoreCase))
                {
                    return post;
                }
            }

            return null;
        }
    }
}
