﻿using System;
using System.Diagnostics;

namespace Drudoca.Blog.Data
{
    [DebuggerDisplay("{Title}")]
    public class BlogPostData
    {

        public BlogPostData(
            string title,
            string author,
            DateTime publishedOn,
            bool isPublished,
            bool isListed,
            string markdown)
        {
            Title = title;
            Author = author;
            PublishedOn = publishedOn;
            IsPublished = isPublished;
            IsListed = isListed;
            Markdown = markdown;
        }

        public string Title { get; }
        public string Author { get; }
        public DateTime PublishedOn { get; }
        public bool IsPublished { get; }
        public bool IsListed { get; }
        public string Markdown { get; }
    }
}