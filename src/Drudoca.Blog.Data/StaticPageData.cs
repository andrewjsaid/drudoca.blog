using System.Diagnostics;

namespace Drudoca.Blog.Data;

[DebuggerDisplay("{" + nameof(FileName) + "}")]
public class StaticPageData
{
    public required string FileName { get; init; }

    public required string Title { get; init; }

    public required string UriSegment { get; init; }

    public required bool IsPublished { get; init; }

    public required int? MenuSequence { get; init; }

    public required string? MenuIcon { get; init; }

    public required string? MenuText { get; init; }

    public required string Markdown { get; init; }

    public required PageMetadata PageMetadata { get; init; }
}