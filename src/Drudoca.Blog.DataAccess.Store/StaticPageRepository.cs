using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class StaticPageRepository : IStaticPageRepository
    {
        private readonly IMarkdownStore<StaticPageData> _store;

        public StaticPageRepository(IMarkdownStore<StaticPageData> store)
        {
            _store = store;
        }

        public async IAsyncEnumerable<StaticPageData> GetAllAsync()
        {
            var posts = await _store.GetAllAsync();

            foreach (var post in posts)
            {
                yield return post;
            }
        }

        public async Task<StaticPageData?> GetByUriSegmentAsync(string uriSegment)
        {
            var posts = await _store.GetAllAsync();

            foreach (var post in posts)
            {
                if (string.Equals(uriSegment, post.UriSegment, StringComparison.OrdinalIgnoreCase))
                {
                    return post;
                }
            }

            return null;
        }
    }
}
