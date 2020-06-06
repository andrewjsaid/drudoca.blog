using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Drudoca.Blog.Data;
using Drudoca.Blog.DataAccess;
using Drudoca.Blog.Domain.Notifications;
using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.Domain
{
    internal class BlogService : IBlogService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IPostBuilder _postBuilder;
        private readonly ICommentBuilder _commentBuilder;
        private readonly IEmailService _emailService;
        private readonly INotificationBuilder _notificationBuilder;
        private readonly BlogOptions _options;
        private readonly ILogger _logger;

        public BlogService(
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IPostBuilder postBuilder,
            ICommentBuilder commentBuilder,
            IEmailService emailService,
            INotificationBuilder notificationBuilder,
            BlogOptions options,
            ILogger<BlogService> logger)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _postBuilder = postBuilder;
            _commentBuilder = commentBuilder;
            _emailService = emailService;
            _notificationBuilder = notificationBuilder;
            _options = options;
            _logger = logger;
        }

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

        public async Task<BlogPage> GetPageAsync(int pageSize, int pageNum)
        {
            if (pageNum <= 0) throw new ArgumentOutOfRangeException(nameof(pageNum), "pageNum must not be less than 1");

            int skip = pageSize * (pageNum - 1);

            Debug.Assert(pageSize > 0);
            var pagePosts = new List<BlogPagePost>(pageSize);

            // We can't use _repository.CountBlogPostsAsync() since
            // it includes "hidden" posts.
            var count = 0;
            var stop = false;

            var showFuture = _options.ListFuturePosts;

            await foreach (var postData in _postRepository.GetAllPostsAsync())
            {
                if (!ShowInListing(postData, showFuture))
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
                    var post = _postBuilder.Build(postData);
                    var numComments = await _commentRepository.CountByPostAsync(post.FileName);
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

            await foreach (var postData in _postRepository.GetPostsByDateAsync(published))
            {
                if (postData.IsPublished)
                {
                    var postSlug = UrlSlug.Slugify(postData.Title);
                    if (string.Equals(postSlug, slug, StringComparison.OrdinalIgnoreCase))
                    {
                        var post = _postBuilder.Build(postData);
                        return post;
                    }
                }
            }

            _logger.LogDebug("Post with date {date} and slug {slug} not found", published, slug);
            return null;
        }

        public async Task<int> CountCommentsAsync(string postFileName)
        {
            var result = await _commentRepository.CountByPostAsync(postFileName);
            return result;
        }

        public async Task<BlogComment[]> GetCommentsAsync(string postFileName)
        {
            var commentData = await _commentRepository.GetByPostAsync(postFileName);

            _logger.LogDebug("Found {count} comments for {post}", commentData.Length, postFileName);

            var results = _commentBuilder.BuildTree(commentData);

            return results;
        }

        public async Task<long?> CreateCommentAsync(string postFileName, long? parentId, string author, string email, string markdown)
        {
            var post = await _postRepository.GetPostByFileName(postFileName);
            if (post == null)
            {
                _logger.LogWarning("Post with file name '{file-name}' was not found", postFileName);
                return null;
            }

            if (parentId != null)
            {
                var parent = await _commentRepository.GetAsync(parentId.Value);
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

            var comment = new CommentData(
                0,
                postFileName,
                parentId,
                author,
                email,
                markdown,
                DateTime.UtcNow,
                false);

            var id = await _commentRepository.CreateAsync(comment);

            _logger.LogDebug("Created comment {id} for post {post} with author: {author}", id, comment.PostFileName, comment.Author);

            await NotifyOnCommentAsync(post, comment);

            return id;
        }

        public async Task DeleteCommentAsync(long id)
        {
            await _commentRepository.MarkDeletedAsync(id);

            _logger.LogDebug("Marked comment {id} as deleted (if it exists)");
        }

        private async Task NotifyOnCommentAsync(PostData post, CommentData comment)
        {
            CommentData? parent = null;
            if (comment.ParentId != null)
            {
                parent = await _commentRepository.GetAsync(comment.ParentId.Value);
                if (parent == null)
                {
                    _logger.LogWarning(
                        "Parent {parent-id} for comment {comment-id} was not found so could not be notified.",
                        comment.ParentId, comment.Id);
                }
            }

            if (!string.IsNullOrWhiteSpace(parent?.Email))
            {
                var notification = await _notificationBuilder.BlogCommentReplyAsync(post, comment, parent);
                await _emailService.SendEmailAsync(parent.Email, notification);
            }

            if (!string.IsNullOrEmpty(post.Email))
            {
                var notification = await _notificationBuilder.BlogCommentAuthorAsync(post, comment);
                await _emailService.SendEmailAsync(post.Email, notification);
            }
        }
    }
}
