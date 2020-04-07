namespace Drudoca.Blog.Config
{
    public class BlogOptions
    {

        public int BlogCacheDurationMins { get; set; } = 10;
        public string BlogFolder { get; set; } = "blog-posts";
        public int PageSize { get; set; } = 3;

    }
}
