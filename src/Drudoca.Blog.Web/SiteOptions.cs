namespace Drudoca.Blog.Web;

public class SiteOptions
{
    public string Url { get; set; } = default!;
    public string[] Administrators { get; set; } = [];
    public string? DataProtectionPath { get; set; }
    public string? GoogleTagManagerClientId { get; set; }
    public string? FontAwesomeId { get; set; }
    public string? ThemeColor { get; set; }

    public Dictionary<string, string> MetaTags { get; set; } = new();
}