using System;
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
    }
}
