using System.Diagnostics;

namespace Drudoca.Blog.Domain
{
    [DebuggerDisplay("Page {" + nameof(PageNum) + "}")]
    public class BlogPage
    {

        public BlogPage(
            int pageNum,
            int pageSize,
            int pageCount,
            BlogPagePost[] posts)
        {
            PageNum = pageNum;
            PageSize = pageSize;
            PageCount = pageCount;
            Posts = posts;
        }

        public int PageNum { get; }
        public int PageSize { get; }
        public int PageCount { get; }
        public BlogPagePost[] Posts { get; }
    }
}
