namespace Drudoca.Blog.Config
{
    public class BlogOptions
    {
        public int BlogCacheDurationMins { get; set; } = 10;
        public int PageSize { get; set; } = 3;
        public bool ListFuturePosts { get; set; } = false;
    }
}
