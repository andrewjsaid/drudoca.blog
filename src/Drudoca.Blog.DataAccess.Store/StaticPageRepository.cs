using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess.Store;

internal class StaticPageRepository(IMarkdownStore<StaticPageData> store) : IStaticPageRepository
{
    public async IAsyncEnumerable<StaticPageData> GetAllAsync()
    {
        var posts = await store.GetAllAsync();

        foreach (var post in posts)
        {
            yield return post;
        }
    }

    public async Task<StaticPageData?> GetByUriSegmentAsync(string uriSegment)
    {
        var posts = await store.GetAllAsync();

        foreach (var post in posts)
        {
            if (string.Equals(uriSegment, post.UriSegment, StringComparison.OrdinalIgnoreCase))
            {
                return post;
            }
        }

        return null;
    }

    public async Task<bool> HasPageAsync(string uriSegment)
    {
        var posts = await store.GetAllAsync();

        foreach (var post in posts)
        {
            if (string.Equals(uriSegment, post.UriSegment, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}