using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain.Notifications;

public interface INotificationBuilder
{
    Task<NotificationEvent> BlogCommentReplyAsync(PostData post, CommentData comment, CommentData parent);
    Task<NotificationEvent> BlogCommentAuthorAsync(PostData post, CommentData comment);
}