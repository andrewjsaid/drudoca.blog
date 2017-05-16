using System.Diagnostics;

namespace Drudoca.Blog.Models
{
    [DebuggerDisplay("Count={Posts.Length}, Pages={NumPages}")]
    public class BlogPosts
    {
        public BlogPost[] Posts { get; set; }
        public int NumPages { get; set; }
    }
}
