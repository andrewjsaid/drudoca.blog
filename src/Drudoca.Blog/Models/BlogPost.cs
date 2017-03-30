using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Drudoca.Blog.Models
{
    [DebuggerDisplay("{Title}")]
    public class BlogPost
    {
        public string Title { get; set; }
        public string Markdown { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Slug { get; set; }

        public static IComparer<BlogPost> MostRecentFirstComparer { get; } = new MyMostRecentFirstComparer();

        internal class MyMostRecentFirstComparer : IComparer<BlogPost>
        {
            public int Compare(BlogPost x, BlogPost y) => y.PublishedDate.CompareTo(x.PublishedDate);
        }
    }
}
