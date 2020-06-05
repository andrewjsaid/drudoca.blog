using System.Diagnostics;

namespace Drudoca.Blog.Domain
{
    [DebuggerDisplay("{" + nameof(Post) + "}")]
    public class BlogPagePost
    {
        public BlogPagePost(BlogPost post, int numComments)
        {
            Post = post;
            NumComments = numComments;
        }

        public BlogPost Post { get; }
        public int NumComments { get; }
    }
}