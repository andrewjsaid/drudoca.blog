using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class StoreBlogRepository : IBlogRepository
    {
        private readonly IBlogStore _store;

        public StoreBlogRepository(IBlogStore store)
        {
            _store = store;
        }

        public async Task<int> CountBlogPostsAsync()
        {
            var posts = await _store.GetAllAsync();
            return posts.Length;
        }

        public async IAsyncEnumerable<BlogPostData> GetAllPostsAsync()
        {
            var posts = await _store.GetAllAsync();

            foreach (var post in posts)
            {
                yield return post;
            }
        }

        public async IAsyncEnumerable<BlogPostData> GetPostsByDateAsync(DateTime published)
        {
            var posts = await _store.GetAllAsync();

            foreach (var post in posts)
            {
                if (post.PublishedOn.Date == published)
                {
                    yield return post;
                }
            }
        }
    }
}
