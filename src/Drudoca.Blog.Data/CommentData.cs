﻿using System;

namespace Drudoca.Blog.Data
{
    public class CommentData
    {
        public CommentData(
            Guid id,
            string postFileName,
            Guid? parentId,
            string author,
            string markdown,
            DateTime postedOnUtc,
            bool isDeleted)
        {
            Id = id;
            PostFileName = postFileName;
            ParentId = parentId;
            Author = author;
            Markdown = markdown;
            PostedOnUtc = postedOnUtc;
            IsDeleted = isDeleted;
        }

        public Guid Id { get; }
        public string PostFileName { get; }
        public Guid? ParentId { get; }
        public string Author { get; }
        public string Markdown { get; }
        public DateTime PostedOnUtc { get; }
        public bool IsDeleted { get; }
    }
}
