using System.Diagnostics;

namespace Drudoca.Blog.Domain;

[DebuggerDisplay("Comment by: {" + nameof(Author) + "}")]
public class BlogComment(
    long id,
    string author,
    string email,
    DateTime postedOnUtc,
    string html,
    bool isDeleted,
    BlogComment[]? replies)
{
    public long Id { get; } = id;
    public string Author { get; } = author;
    public string Email { get; } = email;
    public DateTime PostedOnUtc { get; } = postedOnUtc;
    public string Html { get; } = html;
    public bool IsDeleted { get; } = isDeleted;
    public BlogComment[]? Replies { get; } = replies;
}