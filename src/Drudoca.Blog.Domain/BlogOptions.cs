namespace Drudoca.Blog.Domain;

internal class BlogOptions
{
    public int PageSize { get; set; } = 3;

    public bool ListFuturePosts { get; set; } = false;
}