using System.Diagnostics;

namespace Drudoca.Blog.Data;

[DebuggerDisplay("{" + nameof(FileName) + "}")]
public class EmailTemplateData
{
    public required string FileName { get; init; }

    public required string? From { get; init; }

    public required string[] Cc { get; init; }

    public required string[] Bcc { get; init; }

    public required string Subject { get; init; }

    public required bool IsEnabled { get; init; }

    public required ContentsType ContentsType { get; init; }

    public required string Contents { get; init; }
}