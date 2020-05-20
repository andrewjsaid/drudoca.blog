namespace Drudoca.Blog.Config
{
    public class StoreOptions
    {
        public int CacheDurationMins { get; set; } = 10;
        public string BlogPostPath { get; set; } = default!;
        public string StaticPagePath { get; set; } = default!;
        public string ContentImagesPath { get; set; } = default!;
    }
}
