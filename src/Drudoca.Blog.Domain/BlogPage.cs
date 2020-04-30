using System.Diagnostics;

namespace Drudoca.Blog.Domain
{
    [DebuggerDisplay("Page {PageNum}")]
    public class BlogPage
    {

        public BlogPage(
            int pageNum,
            int pageSize,
            int pageCount,
            BlogPost[] posts)
        {
            PageNum = pageNum;
            PageSize = pageSize;
            PageCount = pageCount;
            Posts = posts;
        }

        public int PageNum { get; }
        public int PageSize { get; }
        public int PageCount { get; }
        public BlogPost[] Posts { get; }
    }
}
