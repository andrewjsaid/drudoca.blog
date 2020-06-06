using System.Collections.Generic;
using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain.Notifications
{
    internal class NotificationBuilder : INotificationBuilder
    {
        private readonly SiteOptions _siteOptions;
        private readonly IMarkdownParser _markdownParser;

        public NotificationBuilder(SiteOptions siteOptions, IMarkdownParser markdownParser)
        {
            _siteOptions = siteOptions;
            _markdownParser = markdownParser;
        }

        private string GetPostUrl(PostData post) => $"{_siteOptions.Url}/{post.PublishedOn.Year}/{post.PublishedOn.Month}/{post.PublishedOn.Day}/{UrlSlug.Slugify(post.Title)}";
        private string GetCommentUrl(PostData post, CommentData comment) => $"{GetPostUrl(post)}#c{comment.Id}";

        public Task<NotificationEvent> BlogCommentReplyAsync(PostData post, CommentData comment, CommentData parent)
        {
            var notificationEvent = new NotificationEvent(
                "blog.comment.reply", new Dictionary<string, string>
                {
                    {"post.url", GetPostUrl(post)},
                    {"post.title", post.Title},
                    {"post.author", post.Author},
                    {"comment.author", comment.Author},
                    {"comment.url", GetCommentUrl(post, comment)},
                    {"comment.html", _markdownParser.ToUntrustedHtml(comment.Markdown)},
                    {"parent.author", comment.Author},
                    {"parent.url", GetCommentUrl(post, parent)},
                });
            return Task.FromResult(notificationEvent);
        }

        public Task<NotificationEvent> BlogCommentAuthorAsync(PostData post, CommentData comment)
        {
            var notificationEvent = new NotificationEvent(
                "blog.comment.author", new Dictionary<string, string>
                {
                    {"post.url", GetPostUrl(post)},
                    {"post.title", post.Title},
                    {"post.author", post.Author},
                    {"comment.author", comment.Author},
                    {"comment.url", GetCommentUrl(post, comment)},
                    {"comment.html", _markdownParser.ToUntrustedHtml(comment.Markdown)},
                });
            return Task.FromResult(notificationEvent);
        }
    }
}
