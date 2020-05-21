using System.Diagnostics;

namespace Drudoca.Blog.Data
{
    [DebuggerDisplay("{" + nameof(FileName) + "}")]
    public class StaticPageData
    {
        public StaticPageData(
            string fileName,
            string title,
            string uriSegment,
            bool isPublished,
            int? menuSequence,
            string? menuIcon,
            string? menuText,
            string markdown)
        {
            FileName = fileName;
            Title = title;
            UriSegment = uriSegment;
            IsPublished = isPublished;
            MenuSequence = menuSequence;
            MenuIcon = menuIcon;
            MenuText = menuText;
            Markdown = markdown;
        }

        public string FileName { get; }
        public string Title { get; }
        public string UriSegment { get; }
        public bool IsPublished { get; }
        public int? MenuSequence { get; }
        public string? MenuIcon { get; }
        public string? MenuText { get; }
        public string Markdown { get; }
    }
}
