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
        private readonly IBlogPostSource _blogPostSource;

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

        public async ValueTask<BlogPost> GetBlogPost(int year, int month, string slug)
        {
            _logger.LogDebug("Retrieving post {year}/{month}/{slug}", year, month, slug);

            var allPosts = await GetBlogPostsAsync();

            foreach (var blogPost in allPosts)
            {
                if(blogPost.Date.Year == year
                    && blogPost.Date.Month == month
                    && blogPost.Slug == slug)
                {
                    return blogPost;
                }
            }

            return null;
        }

        public async ValueTask<BlogPosts> GetBlogPostsAsync(int pageNum)
        {
            if (pageNum <= 0) throw new ArgumentOutOfRangeException(nameof(pageNum), "pageNum must not be less than 0");

            var pageSize = _siteOptions.Value.PageSize;

            _logger.LogDebug("Retrieving {page-size} posts on page {page-num}", pageSize, pageNum);
            
            int skip = pageSize * (pageNum - 1);

            var blogPosts = await GetBlogPostsAsync();

            var results = new BlogPost[Math.Min(pageSize, blogPosts.Length - skip)];
            Array.Copy(blogPosts, skip, results, 0, results.Length);

            var numPages = (int)Math.Ceiling(blogPosts.Length / (double)pageSize);

            var result = new BlogPosts
            {
                Posts = results,
                NumPages = numPages
            };
            return result;
        }
        
        private async ValueTask<BlogPost[]> GetBlogPostsAsync()
        {
            async Task<BlogPost[]> LoadFromSourceAsync()
            {
                var loaded = await _blogPostSource.LoadAsync();
                Debug.Assert(loaded != null, "loaded != null");
                _logger.LogInformation("Loaded {count} items from source", loaded.Length);
                return loaded;
            }

            var isCachingPosts = _siteOptions.Value.IsCachingPosts;
            if (!isCachingPosts)
            {
                return await LoadFromSourceAsync();
            }

            BlogPost[] results;
            if(_memoryCache.TryGetValue(_blogPostsCacheKey, out results))
            {
                return results;
            }

            results = await LoadFromSourceAsync();
            _memoryCache.Set(_blogPostsCacheKey, results);
            return results;
        }
    }
}