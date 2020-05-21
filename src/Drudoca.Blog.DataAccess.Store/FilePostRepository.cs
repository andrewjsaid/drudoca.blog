using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class FilePostRepository : IPostRepository
    {
        private readonly IMarkdownStore<PostData> _store;

        public FilePostRepository(IMarkdownStore<PostData> store)
        {
            _store = store;
        }

        public async Task<int> CountPostsAsync()
        {
            var posts = await _store.GetAllAsync();
            return posts.Length;
        }

        public async IAsyncEnumerable<PostData> GetAllPostsAsync()
        {
            var posts = await _store.GetAllAsync();

            foreach (var post in posts)
            {
                yield return post;
            }
        }

        public async IAsyncEnumerable<PostData> GetPostsByDateAsync(DateTime published)
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
