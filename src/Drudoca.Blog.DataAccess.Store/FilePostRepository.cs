using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess.Store;

internal class FilePostRepository(IMarkdownStore<PostData> store) : IPostRepository
{
    public async Task<int> CountPostsAsync()
    {
        var posts = await store.GetAllAsync();
        return posts.Length;
    }

    public async IAsyncEnumerable<PostData> GetAllPostsAsync()
    {
        var posts = await store.GetAllAsync();

        foreach (var post in posts)
        {
            yield return post;
        }
    }

    public async Task<PostData?> GetPostByFileName(string fileName)
    {
        var posts = await store.GetAllAsync();

        foreach (var post in posts)
        {
            if (string.Equals(post.FileName, fileName, StringComparison.OrdinalIgnoreCase))
            {
                return post;
            }
        }

        return null;
    }

    public async IAsyncEnumerable<PostData> GetPostsByDateAsync(DateTime published)
    {
        var posts = await store.GetAllAsync();

        foreach (var post in posts)
        {
            if (post.PublishedOn.Date == published)
            {
                yield return post;
            }
        }
    }
}