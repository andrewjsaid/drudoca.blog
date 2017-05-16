using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Drudoca.Blog.Models
{
    [DebuggerDisplay("{Title}")]
    public class BlogPost
    {
        public string FileName { get; set; }

        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public bool IsPublished { get; set; }

        public string Slug { get; set; }
        public string IntroHtml { get; set; }
        public string FullHtml { get; set; }

        public static IComparer<BlogPost> MostRecentFirstComparer { get; } = new MyMostRecentFirstComparer();

        internal class MyMostRecentFirstComparer : IComparer<BlogPost>
        {
            public int Compare(BlogPost x, BlogPost y) => y.Date.CompareTo(x.Date);
        }
    }
}
