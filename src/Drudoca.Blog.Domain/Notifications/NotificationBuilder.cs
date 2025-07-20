using Drudoca.Blog.Data;
using Microsoft.Extensions.Options;

namespace Drudoca.Blog.Domain.Notifications;

internal class NotificationBuilder(IOptions<NotificationOptions> options, IMarkdownParser markdownParser) : INotificationBuilder
{
    private string GetPostUrl(PostData post) => $"{options.Value.SiteUrl}/{post.PublishedOn.Year}/{post.PublishedOn.Month}/{post.PublishedOn.Day}/{UrlSlug.Slugify(post.Title)}";

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
                {"comment.html", markdownParser.UntrustedToHtml(comment.Markdown)},
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
                {"comment.html", markdownParser.UntrustedToHtml(comment.Markdown)},
            });
        return Task.FromResult(notificationEvent);
    }
}