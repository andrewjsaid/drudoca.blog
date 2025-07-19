using System.Diagnostics;

namespace Drudoca.Blog.Domain;

[DebuggerDisplay("{" + nameof(FileName) + "}")]
public class StaticPage(
    string fileName,
    string title,
    string uriSegment,
    bool isPublished,
    string html,
    PageMetadata pageMetadata)
{
    public string FileName { get; } = fileName;
    public string Title { get; } = title;
    public string UriSegment { get; } = uriSegment;
    public bool IsPublished { get; } = isPublished;
    public string Html { get; } = html;

    public PageMetadata PageMetadata { get; } = pageMetadata;
}