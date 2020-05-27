using System;
using System.Diagnostics;

namespace Drudoca.Blog.Data
{
    [DebuggerDisplay("{" + nameof(FileName) + "}")]
    public class PostData
    {

        public PostData(
            string fileName,
            string title,
            string author,
            DateTime publishedOn,
            bool isPublished,
            bool isListed,
            string markdown,
            PageMetaData pageMetaData)
        {
            FileName = fileName;
            Title = title;
            Author = author;
            PublishedOn = publishedOn;
            IsPublished = isPublished;
            IsListed = isListed;
            Markdown = markdown;
            PageMetaData = pageMetaData;
        }

        public string FileName { get; }
        public string Title { get; }
        public string Author { get; }
        public DateTime PublishedOn { get; }
        public bool IsPublished { get; }
        public bool IsListed { get; }
        public string Markdown { get; }
        public PageMetaData PageMetaData { get; }
    }
}