using System.Diagnostics;

namespace Drudoca.Blog.Domain
{
    [DebuggerDisplay("{" + nameof(FileName) + "}")]
    public class StaticPage
    {

        public StaticPage(
            string fileName,
            string title,
            string uriSegment,
            bool isPublished,
            string html)
        {
            FileName = fileName;
            Title = title;
            UriSegment = uriSegment;
            IsPublished = isPublished;
            Html = html;
        }

        public string FileName { get; }
        public string Title { get; }
        public string UriSegment { get; }
        public bool IsPublished { get; }
        public string Html { get; }
    }
}
