using System;

namespace Drudoca.Blog.Data
{
    public class CommentData
    {
        public CommentData(
            long id,
            string postFileName,
            long? parentId,
            string author,
            string email,
            string markdown,
            DateTime postedOnUtc,
            bool isDeleted)
        {
            Id = id;
            PostFileName = postFileName;
            ParentId = parentId;
            Author = author;
            Email = email;
            Markdown = markdown;
            PostedOnUtc = postedOnUtc;
            IsDeleted = isDeleted;
        }

        public long Id { get; }
        public string PostFileName { get; }
        public long? ParentId { get; }
        public string Author { get; }
        public string Email { get; }
        public string Markdown { get; }
        public DateTime PostedOnUtc { get; }
        public bool IsDeleted { get; }
    }
}
