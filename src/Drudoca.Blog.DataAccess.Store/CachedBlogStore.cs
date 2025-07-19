using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Drudoca.Blog.DataAccess.Store;

internal class CachedMarkdownStore<T>(
    IMemoryCache memoryCache,
    IOptions<StoreOptions> storeOptions,
    IMarkdownDirectoryReader reader,
    IMarkdownFileConverter<T> converter,
    ILogger<MarkdownStore<T>> logger)
    : MarkdownStore<T>(reader, converter, logger)
    where T : class
{
    private static readonly object _cacheKey = new ();

    public override async ValueTask<T[]> GetAllAsync()
    {
        if (memoryCache.TryGetValue<CachedBlogPosts?>(_cacheKey, out var cacheItem)
            && cacheItem is not null)
            return cacheItem.Data;

        var data = await base.GetAllAsync();
        var cacheMins = storeOptions.Value.CacheDurationMins;

        if (cacheMins == 0)
        {
            return data;
        }

        cacheItem = new CachedBlogPosts(data);
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheMins),
        };
        memoryCache.Set(_cacheKey, cacheItem, options);

        return cacheItem.Data;
    }

    private class CachedBlogPosts(T[] data)
    {
        public T[] Data { get; } = data;
    }
}