using System;
using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class CachedMarkdownStore<T> : MarkdownStore<T> where T : class
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object _cacheKey = new object();

        private readonly IMemoryCache _memoryCache;
        private readonly StoreOptions _storeOptions;

        public CachedMarkdownStore(
            IMemoryCache memoryCache,
            StoreOptions storeOptions,
            IMarkdownDirectoryReader reader,
            IMarkdownFileConverter<T> converter,
            ILogger<MarkdownStore<T>> logger)
            : base(reader, converter, logger)
        {
            _memoryCache = memoryCache;
            _storeOptions = storeOptions;
        }

        public override async ValueTask<T[]> GetAllAsync()
        {
            if (_memoryCache.TryGetValue<CachedBlogPosts?>(_cacheKey, out var cacheItem)
                && cacheItem is not null)
                return cacheItem.Data;

            var data = await base.GetAllAsync();
            var cacheMins = _storeOptions.CacheDurationMins;

            if (cacheMins == 0)
            {
                return data;
            }

            cacheItem = new CachedBlogPosts(data);
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheMins),
            };
            _memoryCache.Set(_cacheKey, cacheItem, options);

            return cacheItem.Data;
        }

        private class CachedBlogPosts
        {
            public CachedBlogPosts(
                T[] data)
            {
                Data = data;
            }

            public T[] Data { get; }
        }
    }
}
