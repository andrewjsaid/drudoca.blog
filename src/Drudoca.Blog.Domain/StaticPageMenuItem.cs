using System.Diagnostics;

namespace Drudoca.Blog.Domain;

[DebuggerDisplay("{" + nameof(Text) + "}")]
public class StaticPageMenuItem(
    int sequence,
    string text,
    string? icon,
    string uriSegment)
{
    public int Sequence { get; } = sequence;
    public string Text { get; } = text;
    public string? Icon { get; } = icon;
    public string UriSegment { get; } = uriSegment;
}