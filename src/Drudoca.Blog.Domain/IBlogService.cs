namespace Drudoca.Blog.Domain;

public interface IBlogService
{
    Task<BlogPage> GetPageAsync(int pageNum);
    Task<BlogPost?> GetPostAsync(DateTime published, string slug);
    Task<BlogPost?> GetMostRecentPostAsync();
    Task<(BlogPost? earlier, BlogPost? later)> GetSurroundingPostsAsync(string fileName);

    Task<int> CountCommentsAsync(string postFileName);
    Task<BlogComment[]> GetCommentsAsync(string postFileName);
    Task<long?> CreateCommentAsync(string postFileName, long? parentId, string author, string email, string markdown);
    Task DeleteCommentAsync(long id);
}