using System;
using System.Diagnostics;

namespace Drudoca.Blog.ViewModels
{
    [DebuggerDisplay("{Slug}")]
    public class BlogPostViewModel
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string RawHtml { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
