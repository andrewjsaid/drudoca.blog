using System.Diagnostics;

namespace Drudoca.Blog.Domain;

[DebuggerDisplay("Page {" + nameof(PageNum) + "}")]
public class BlogPage(
    int pageNum,
    int pageSize,
    int pageCount,
    BlogPagePost[] posts)
{
    public int PageNum { get; } = pageNum;
    public int PageSize { get; } = pageSize;
    public int PageCount { get; } = pageCount;
    public BlogPagePost[] Posts { get; } = posts;
}