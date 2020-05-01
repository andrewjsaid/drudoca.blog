using System;
using System.Diagnostics;

namespace Drudoca.Blog.Domain
{
    [DebuggerDisplay("Comment by: {Author}")]
    public class BlogComment
    {
        public BlogComment(
            Guid id,
            string author,
            string email,
            DateTime postedOnUtc,
            string html,
            bool isDeleted,
            BlogComment[]? children)
        {
            Id = id;
            Author = author;
            Email = email;
            PostedOnUtc = postedOnUtc;
            Html = html;
            IsDeleted = isDeleted;
            Children = children;
        }

        public Guid Id { get; }
        public string Author { get; }
        public string Email { get; }
        public DateTime PostedOnUtc { get; }
        public string Html { get; }
        public bool IsDeleted { get; }
        public BlogComment[]? Children { get; }
    }
}
