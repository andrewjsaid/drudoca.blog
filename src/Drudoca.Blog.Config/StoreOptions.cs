namespace Drudoca.Blog.Config
{
    public class StoreOptions
    {
        public int CacheDurationMins { get; set; } = 10;
        public string BlogPostPath { get; set; } = "blog-posts";
        public string StaticPagePath { get; set; } = "static-pages";
    }
}
