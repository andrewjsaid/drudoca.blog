using System;
using System.Diagnostics;

namespace Drudoca.Blog.Domain
{
    [DebuggerDisplay("Comment by: {" + nameof(Author) + "}")]
    public class BlogComment
    {
        public BlogComment(
            long id,
            string author,
            string email,
            DateTime postedOnUtc,
            string html,
            bool isDeleted,
            BlogComment[]? replies)
        {
            Id = id;
            Author = author;
            Email = email;
            PostedOnUtc = postedOnUtc;
            Html = html;
            IsDeleted = isDeleted;
            Replies = replies;
        }

        public long Id { get; }
        public string Author { get; }
        public string Email { get; }
        public DateTime PostedOnUtc { get; }
        public string Html { get; }
        public bool IsDeleted { get; }
        public BlogComment[]? Replies { get; }
    }
}
