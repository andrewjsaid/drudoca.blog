using System.Diagnostics;

namespace Drudoca.Blog.Domain;

[DebuggerDisplay("{" + nameof(Title) + "}")]
public class BlogPost(
    string fileName,
    string title,
    string author,
    DateTime publishedOn,
    bool isPublished,
    bool isListed,
    string slug,
    string html,
    string? introHtml,
    PageMetadata pageMetadata)
{
    /// <summary>
    /// Serves as an identifier for the post.
    /// </summary>
    public string FileName { get; } = fileName;

    public string Title { get; } = title;
    public string Author { get; } = author;
    public DateTime PublishedOn { get; } = publishedOn;
    public bool IsPublished { get; } = isPublished;
    public bool IsListed { get; } = isListed;
    public string Slug { get; } = slug;
    public string Html { get; } = html;
    public string? IntroHtml { get; } = introHtml;
    public PageMetadata PageMetadata { get; } = pageMetadata;
}