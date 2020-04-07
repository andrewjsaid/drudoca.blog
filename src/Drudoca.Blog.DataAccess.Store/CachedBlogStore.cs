using System;
using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Drudoca.Blog.Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class CachedBlogStore : BlogStore
    {
        private static readonly object _cacheKey = new object();

        private readonly IMemoryCache _memoryCache;
        private readonly IOptionsMonitor<BlogOptions> _siteOptions;

        public CachedBlogStore(
            IMemoryCache memoryCache,
            ILogger<BlogStore> logger,
            IOptionsMonitor<BlogOptions> siteOptions)
            : base(logger, siteOptions)
        {
            _memoryCache = memoryCache;
            _siteOptions = siteOptions;
        }

        public async override ValueTask<BlogPostData[]> GetAllAsync()
        {
            if (!_memoryCache.TryGetValue<CachedBlogPosts>(_cacheKey, out var cacheItem))
            {
                var posts = await base.GetAllAsync();
                cacheItem = new CachedBlogPosts(posts);
                var cacheMins = _siteOptions.CurrentValue.BlogCacheDurationMins;
                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheMins),
                };
                _memoryCache.Set(_cacheKey, cacheItem, options);
            }
            return cacheItem.Posts;
        }

        private class CachedBlogPosts
        {
            public CachedBlogPosts(
                BlogPostData[] posts)
            {
                Posts = posts;
            }

            public BlogPostData[] Posts { get; }
        }
    }
}
