namespace Drudoca.Blog.Domain;

internal class BlogOptions
{
    public int PageSize { get; set; } = 5;

    public bool ListFuturePosts { get; set; } = false;
}