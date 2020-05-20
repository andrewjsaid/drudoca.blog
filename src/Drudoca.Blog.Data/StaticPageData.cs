using System.Diagnostics;

namespace Drudoca.Blog.Data
{
    [DebuggerDisplay("{" + nameof(FileName) + "}")]
    public class StaticPageData
    {
        public StaticPageData(
            string fileName,
            string uriSegment,
            string? menuIcon,
            string? menuText,
            bool isPublished,
            int sequence, 
            string markdown)
        {
            FileName = fileName;
            UriSegment = uriSegment;
            MenuIcon = menuIcon;
            MenuText = menuText;
            IsPublished = isPublished;
            Sequence = sequence;
            Markdown = markdown;
        }

        public string FileName { get; }
        public string UriSegment { get; }
        public string? MenuIcon { get; }
        public string? MenuText { get; }
        public bool IsPublished { get; }
        public int Sequence { get; }
        public string Markdown { get; }
    }
}
