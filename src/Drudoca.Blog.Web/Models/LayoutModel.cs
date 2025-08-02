using Drudoca.Blog.Domain;

namespace Drudoca.Blog.Web.Models;

public class LayoutModel(
    StaticPageMenuItem[] menu,
    SeoOptions seo,
    string? fontAwesomeId,
    string? themeColor,
    IReadOnlyDictionary<string, string> metaTags)
{
    public StaticPageMenuItem[] Menu { get; } = menu;
    public SeoOptions Seo { get; set; } = seo;
    public string? FontAwesomeId { get; } = fontAwesomeId;
    public string? ThemeColor { get; set; } = themeColor;
    public IReadOnlyDictionary<string, string> MetaTags { get; } = metaTags;
}