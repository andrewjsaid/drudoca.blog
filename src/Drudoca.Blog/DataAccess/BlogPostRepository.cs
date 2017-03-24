using Drudoca.Blog.Models;
using Drudoca.Blog.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Drudoca.Blog.DataAccess
{
    internal class BlogPostRepository : IBlogPostRepository
    {
        private const int GetBlogPostLogEvent = 1;
        private const int GetBlogPostsLogEvent = 2;

        private const int FirstTimeLogEvent = 3;
        private const int ExpiredLogEvent = 4;
        private const int LoadedLogEvent = 5;

        private const int ExpiryMins = 60;

        private static readonly object _blogPostsCacheKey = new object();

        private readonly ILogger _logger;
        private readonly IOptions<FormattingOptions> _formatOptions;
        private readonly IMemoryCache _memoryCache;
        private IBlogPostSource _blogPostSource;

        public BlogPostRepository(
            ILogger<BlogPostRepository> logger,
            IOptions<FormattingOptions> formatOptions,
            IMemoryCache memoryCache,
            IBlogPostSource blogPostSource)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _formatOptions = formatOptions ?? throw new ArgumentNullException(nameof(formatOptions));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _blogPostSource = blogPostSource ?? throw new ArgumentNullException(nameof(blogPostSource));
        }

        public ValueTask<BlogPost> GetBlogPost(string slug)
        {
            _logger.LogDebug(GetBlogPostLogEvent, "Retrieving post with slug {SLUG}", slug);

            var result = new BlogPost
            {
                Title = "Blog Post 1",
                Markdown = "This is the first post",
                PublishedDate = new DateTime(2017, 03, 21)
            };
            return new ValueTask<BlogPost>(result);
        }

        public async ValueTask<BlogPost[]> GetBlogPostsAsync(int pageNum)
        {
            if (pageNum <= 0) throw new ArgumentOutOfRangeException(nameof(pageNum), "pageNum must not be less than 0");

            var formatOptions = _formatOptions.Value;

            _logger.LogDebug(GetBlogPostsLogEvent, "Retrieving {PAGESIZE} posts on page {PAGENUM}", formatOptions.PageSize, pageNum);

            var pageSize = _formatOptions.Value.PageSize;
            int skip = pageSize * (pageNum - 1);

            var blogPosts = await GetCachedBlogPostsAsync();

            var results = new BlogPost[Math.Min(pageSize, blogPosts.Length - skip)];
            Array.Copy(blogPosts, skip, results, 0, results.Length);

            return results;
        }

        private async ValueTask<BlogPost[]> GetCachedBlogPostsAsync()
        {
            CachedBlogPosts cachedBlogPosts;
            if(_memoryCache.TryGetValue(_blogPostsCacheKey, out cachedBlogPosts))
            {
                if(cachedBlogPosts.Expiry > DateTime.UtcNow)
                {
                    return cachedBlogPosts.BlogPosts;
                }
                else
                {
                    _logger.LogInformation(ExpiredLogEvent, "Cache expired at {EXPIRY} (Now={NOW})", cachedBlogPosts.Expiry, DateTime.UtcNow);
                }
            }
            else
            {
                _logger.LogInformation(FirstTimeLogEvent, "Loading cache for first time");
            }

            var blogPosts = await _blogPostSource.LoadAsync();
            Debug.Assert(blogPosts != null, "blogPosts != null");
            var expiry = DateTime.UtcNow.AddMinutes(ExpiryMins);
            _logger.LogInformation(LoadedLogEvent, "Loaded {COUNT} items from source, expiring on {EXPIRY}", blogPosts.Length, expiry);
            cachedBlogPosts = new CachedBlogPosts(blogPosts, expiry);
            _memoryCache.Set(_blogPostsCacheKey, cachedBlogPosts);
            return cachedBlogPosts.BlogPosts;
        }
    }
}