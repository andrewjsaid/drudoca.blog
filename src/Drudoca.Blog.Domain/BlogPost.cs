using System;
using System.Diagnostics;

namespace Drudoca.Blog.Domain
{
    [DebuggerDisplay("{Title}")]
    public class BlogPost
    {

        public BlogPost(
            string title,
            string author,
            DateTime publishedOn,
            bool isPublished,
            string markdown,
            string slug,
            string html,
            string? introHtml)
        {
            Title = title;
            Author = author;
            PublishedOn = publishedOn;
            IsPublished = isPublished;
            Slug = slug;
            Markdown = markdown;
            Html = html;
            IntroHtml = introHtml;
        }

        public string Title { get; }
        public string Author { get; }
        public DateTime PublishedOn { get; }
        public bool IsPublished { get; }
        public string Markdown { get; }
        public string Slug { get; }
        public string Html { get; }
        public string? IntroHtml { get; }
    }
}