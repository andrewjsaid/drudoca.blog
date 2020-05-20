using System;
using System.Diagnostics;

namespace Drudoca.Blog.Domain
{
    [DebuggerDisplay("{" + nameof(Title) + "}")]
    public class BlogPost
    {

        public BlogPost(
            string fileName,
            string title,
            string author,
            DateTime publishedOn,
            bool isPublished,
            bool isListed,
            string slug,
            string html,
            string? introHtml)
        {
            FileName = fileName;
            Title = title;
            Author = author;
            PublishedOn = publishedOn;
            IsPublished = isPublished;
            IsListed = isListed;
            Slug = slug;
            Html = html;
            IntroHtml = introHtml;
        }

        /// <summary>
        /// Serves as an identifier for the post.
        /// </summary>
        public string FileName { get; }
        public string Title { get; }
        public string Author { get; }
        public DateTime PublishedOn { get; }
        public bool IsPublished { get; }
        public bool IsListed { get; }
        public string Slug { get; }
        public string Html { get; }
        public string? IntroHtml { get; }
    }
}