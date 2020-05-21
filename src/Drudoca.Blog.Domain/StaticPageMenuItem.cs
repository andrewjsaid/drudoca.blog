using System.Diagnostics;

namespace Drudoca.Blog.Domain
{
    [DebuggerDisplay("{" + nameof(Text) + "}")]
    public class StaticPageMenuItem
    {
        public StaticPageMenuItem(
            int sequence,
            string text,
            string? icon,
            string uriSegment)
        {
            Sequence = sequence;
            Text = text;
            Icon = icon;
            UriSegment = uriSegment;
        }

        public int Sequence { get; }
        public string Text { get; }
        public string? Icon { get; }
        public string UriSegment { get; }
    }
}