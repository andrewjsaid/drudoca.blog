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
        private static readonly object _blogPostsCacheKey = new object();

        private readonly ILogger _logger;
        private readonly IOptions<SiteOptions> _siteOptions;
        private readonly IMemoryCache _memoryCache;
        private IBlogPostSource _blogPostSource;

        public BlogPostRepository(
            ILogger<BlogPostRepository> logger,
            IOptions<SiteOptions> siteOptions,
            IMemoryCache memoryCache,
            IBlogPostSource blogPostSource)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _siteOptions = siteOptions ?? throw new ArgumentNullException(nameof(siteOptions));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _blogPostSource = blogPostSource ?? throw new ArgumentNullException(nameof(blogPostSource));
        }

        public ValueTask<BlogPost> GetBlogPost(string slug)
        {
            _logger.LogDebug("Retrieving post with slug {slug}", slug);

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

            var pageSize = _siteOptions.Value.PageSize;

            _logger.LogDebug("Retrieving {page-size} posts on page {page-num}", pageSize, pageNum);
            
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
                    _logger.LogInformation("Cache expired at {expiry} (Now={now})", cachedBlogPosts.Expiry, DateTime.UtcNow);
                }
            }
            else
            {
                _logger.LogInformation("Loading cache for first time");
            }

            var blogPosts = await _blogPostSource.LoadAsync();
            Debug.Assert(blogPosts != null, "blogPosts != null");
            var expiry = DateTime.UtcNow.AddMinutes(_siteOptions.Value.ExpiryMins);
            _logger.LogInformation("Loaded {count} items from source, expiring on {expiry}", blogPosts.Length, expiry);
            cachedBlogPosts = new CachedBlogPosts(blogPosts, expiry);
            _memoryCache.Set(_blogPostsCacheKey, cachedBlogPosts);
            return cachedBlogPosts.BlogPosts;
        }
    }
}