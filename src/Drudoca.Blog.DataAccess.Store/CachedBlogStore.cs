using System;
using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Drudoca.Blog.Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class CachedBlogStore : BlogStore
    {
        private static readonly object _cacheKey = new object();

        private readonly IMemoryCache _memoryCache;
        private readonly BlogOptions _blogOptions;

        public CachedBlogStore(
            IMemoryCache memoryCache,
            ILogger<BlogStore> logger,
            StoreOptions storeOptions,
            BlogOptions blogOptions)
            : base(logger, storeOptions)
        {
            _memoryCache = memoryCache;
            _blogOptions = blogOptions;
        }

        public async override ValueTask<PostData[]> GetAllAsync()
        {
            if (!_memoryCache.TryGetValue<CachedBlogPosts>(_cacheKey, out var cacheItem))
            {
                var posts = await base.GetAllAsync();
                var cacheMins = _blogOptions.BlogCacheDurationMins;

                if (cacheMins == 0)
                {
                    return posts;
                }

                cacheItem = new CachedBlogPosts(posts);
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
                PostData[] posts)
            {
                Posts = posts;
            }

            public PostData[] Posts { get; }
        }
    }
}
