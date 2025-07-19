using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain;

internal interface ICommentBuilder
{
    BlogComment[] BuildTree(CommentData[] data);
}