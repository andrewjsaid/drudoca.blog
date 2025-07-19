namespace Drudoca.Blog.DataAccess.Store;

internal class StoreOptions
{
    public int CacheDurationMins { get; set; } = 10;
    public string? BlogPostPath { get; set; }
    public string? StaticPagePath { get; set; }
    public string? MediaPath { get; set; }
    public string? EmailTemplatePath { get; set; }
}