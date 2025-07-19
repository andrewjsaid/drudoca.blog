using System.Diagnostics;
using Drudoca.Blog.Data;
using Drudoca.Blog.DataAccess;
using Drudoca.Blog.Domain.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Drudoca.Blog.Domain;

internal class BlogService(
    IPostRepository postRepository,
    ICommentRepository commentRepository,
    IPostBuilder postBuilder,
    ICommentBuilder commentBuilder,
    IEmailService emailService,
    INotificationBuilder notificationBuilder,
    IOptions<BlogOptions> options,
    ILogger<BlogService> logger)
    : IBlogService
{
    private readonly ILogger _logger = logger;

    private static bool ShowInListing(PostData post, bool showFuture)
    {
        if (!post.IsListed)
            return false;

        if (!post.IsPublished)
            return false;

        if (!showFuture && post.PublishedOn > DateTime.UtcNow)
            return false;

        return true;
    }

    public async Task<BlogPage> GetPageAsync(int pageNum)
    {
        if (pageNum <= 0) throw new ArgumentOutOfRangeException(nameof(pageNum), "pageNum must not be less than 1");

        var pageSize = options.Value.PageSize;

        Debug.Assert(pageSize > 0);

        int skip = pageSize * (pageNum - 1);

        var pagePosts = new List<BlogPagePost>(pageSize);

        // We can't use _repository.CountBlogPostsAsync() since
        // it includes "hidden" posts.
        var count = 0;
        var stop = false;

        await foreach (var postData in postRepository.GetAllPostsAsync())
        {
            if (!ShowInListing(postData, options.Value.ListFuturePosts))
            {
                continue;
            }

            count++;

            if (stop)
            {
                continue;
            }

            // Keep skipping
            if (skip > 0)
            {
                skip--;
            }
            else
            {
                var post = postBuilder.Build(postData);
                var numComments = await commentRepository.CountByPostAsync(post.FileName);
                pagePosts.Add(new BlogPagePost(post, numComments));

                if (pagePosts.Count == pageSize)
                {
                    stop = true;
                }
            }
        }

        var pageCount = (int)Math.Ceiling(count / (double)pageSize);

        var result = new BlogPage(pageNum, pageSize, pageCount, pagePosts.ToArray());
        return result;
    }

    public async Task<BlogPost?> GetPostAsync(DateTime published, string slug)
    {
        // Allow users to access posts with direct links, even if they are not listed or are future posts
        // Useful for asking friends to have a look at something.
        // We do respect the "IsPublished" flag though.

        await foreach (var postData in postRepository.GetPostsByDateAsync(published))
        {
            if (postData.IsPublished)
            {
                var postSlug = UrlSlug.Slugify(postData.Title);
                if (string.Equals(postSlug, slug, StringComparison.OrdinalIgnoreCase))
                {
                    var post = postBuilder.Build(postData);
                    return post;
                }
            }
        }

        _logger.LogDebug("Post with date {date} and slug {slug} not found", published, slug);
        return null;
    }

    public async Task<BlogPost?> GetMostRecentPostAsync()
    {
        await foreach (var postData in postRepository.GetAllPostsAsync())
        {
            if (!ShowInListing(postData, options.Value.ListFuturePosts))
            {
                continue;
            }

            var result = postBuilder.Build(postData);
            return result;
        }

        return null;
    }

    public async Task<(BlogPost? earlier, BlogPost? later)> GetSurroundingPostsAsync(string fileName)
    {
        PostData? previous = null;
        PostData? next = null;

        var setNext = false;

        await foreach (var postData in postRepository.GetAllPostsAsync())
        {
            if (!ShowInListing(postData, options.Value.ListFuturePosts))
            {
                continue;
            }

            if (setNext)
            {
                next = postData;
                break;
            }

            if (string.Equals(postData.FileName, fileName, StringComparison.OrdinalIgnoreCase))
            {
                setNext = true;
            }
            else
            {
                previous = postData;
            }
        }

        var laterPost = previous == null ? null : postBuilder.Build(previous);
        var earlierPost = next == null ? null : postBuilder.Build(next);
        return (earlierPost, laterPost);
    }

    public async Task<int> CountCommentsAsync(string postFileName)
    {
        var result = await commentRepository.CountByPostAsync(postFileName);
        return result;
    }

    public async Task<BlogComment[]> GetCommentsAsync(string postFileName)
    {
        var commentData = await commentRepository.GetByPostAsync(postFileName);

        _logger.LogDebug("Found {count} comments for {post}", commentData.Length, postFileName);

        var results = commentBuilder.BuildTree(commentData);

        return results;
    }

    public async Task<long?> CreateCommentAsync(string postFileName, long? parentId, string author, string email, string markdown)
    {
        var post = await postRepository.GetPostByFileName(postFileName);
        if (post == null)
        {
            _logger.LogWarning("Post with file name '{file-name}' was not found", postFileName);
            return null;
        }

        if (parentId != null)
        {
            var parent = await commentRepository.GetAsync(parentId.Value);
            if (parent == null)
            {
                _logger.LogWarning("Comment {id} not found", parentId);
                return null;
            }

            if (parent.PostFileName != postFileName)
            {
                _logger.LogWarning("Comment {id} and post {post} do not match", parentId, postFileName);
                return null;
            }

            if (parent.IsDeleted)
            {
                _logger.LogDebug("Attempted to reply to deleted comment {id}", parentId);
                return null;
            }
        }

        var comment = new CommentData
        {
            Id = 0,
            PostFileName = postFileName,
            ParentId = parentId,
            Author = author,
            Email = email,
            Markdown = markdown,
            PostedOnUtc = DateTime.UtcNow,
            IsDeleted = false
        };

        var id = await commentRepository.CreateAsync(comment);

        _logger.LogDebug("Created comment {id} for post {post} with author: {author}", id, comment.PostFileName, comment.Author);

        await NotifyOnCommentAsync(post, comment);

        return id;
    }

    public async Task DeleteCommentAsync(long id)
    {
        await commentRepository.MarkDeletedAsync(id);

        _logger.LogDebug("Marked comment {id} as deleted (if it exists)", id);
    }

    private async Task NotifyOnCommentAsync(PostData post, CommentData comment)
    {
        CommentData? parent = null;
        if (comment.ParentId != null)
        {
            parent = await commentRepository.GetAsync(comment.ParentId.Value);
            if (parent == null)
            {
                _logger.LogWarning(
                    "Parent {parent-id} for comment {comment-id} was not found so could not be notified.",
                    comment.ParentId, comment.Id);
            }
        }

        if (!string.IsNullOrWhiteSpace(parent?.Email))
        {
            var notification = await notificationBuilder.BlogCommentReplyAsync(post, comment, parent);
            await emailService.SendEmailAsync(parent.Email, notification);
        }

        if (!string.IsNullOrEmpty(post.Email))
        {
            var notification = await notificationBuilder.BlogCommentAuthorAsync(post, comment);
            await emailService.SendEmailAsync(post.Email, notification);
        }
    }
}