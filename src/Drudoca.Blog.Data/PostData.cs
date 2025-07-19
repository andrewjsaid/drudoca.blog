using System.Diagnostics;

namespace Drudoca.Blog.Data;

[DebuggerDisplay("{" + nameof(FileName) + "}")]
public class PostData
{
    public required string FileName { get; init; }

    public required string Title { get; init; }

    public required string Author { get; init; }

    public required string? Email { get; init; }

    public required DateTime PublishedOn { get; init; }

    public required bool IsPublished { get; init; }

    public required bool IsListed { get; init; }

    public required string Markdown { get; init; }

    public required PageMetadata PageMetadata { get; init; }
}