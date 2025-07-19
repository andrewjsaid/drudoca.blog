using System.Diagnostics;

namespace Drudoca.Blog.Domain;

[DebuggerDisplay("{" + nameof(Post) + "}")]
public class BlogPagePost(BlogPost post, int numComments)
{
    public BlogPost Post { get; } = post;
    public int NumComments { get; } = numComments;
}