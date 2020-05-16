using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain
{
    internal interface IPostBuilder
    {
        BlogPost Build(PostData data);
    }
}