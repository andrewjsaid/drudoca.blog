namespace Drudoca.Blog.Config
{
    public class StoreOptions
    {
        public int CacheDurationMins { get; set; } = 10;
        public string? BlogPostPath { get; set; }
        public string? StaticPagePath { get; set; }
        public string? ContentImagesPath { get; set; }
    }
}
