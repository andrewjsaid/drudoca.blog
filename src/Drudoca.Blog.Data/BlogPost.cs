using System;
using System.Diagnostics;

namespace Drudoca.Blog.Data
{
    [DebuggerDisplay("{Title}")]
    public sealed class BlogPost
    {

        public BlogPost(
            string title,
            string author,
            DateTime publishedOn,
            bool isPublished,
            string slug,
            string markdown)
        {
            Title = title;
            Author = author;
            PublishedOn = publishedOn;
            IsPublished = isPublished;
            Slug = slug;
            Markdown = markdown;
        }

        public string Title { get; }
        public string Author { get; }
        public DateTime PublishedOn { get; }
        public bool IsPublished { get; }
        public string Slug { get; }
        public string Markdown { get; }
    }
}