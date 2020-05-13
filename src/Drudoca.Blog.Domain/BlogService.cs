using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Drudoca.Blog.Data;
using Drudoca.Blog.DataAccess;
using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.Domain
{
    internal class BlogService : IBlogService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IPostBuilder _postBuilder;
        private readonly ICommentBuilder _commentBuilder;
        private readonly BlogOptions _options;
        private readonly ILogger _logger;

        public BlogService(
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IPostBuilder postBuilder,
            ICommentBuilder commentBuilder,
            BlogOptions options,
            ILogger<BlogService> logger)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _postBuilder = postBuilder;
            _commentBuilder = commentBuilder;
            _options = options;
            _logger = logger;
        }

        private bool ShowInListing(PostData post, bool showFuture)
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
            var pagePosts = new List<BlogPost>(pageSize);

            // We can't use _repository.CountBlogPostsAsync() since
            // it includes "hidden" posts.
            int count = 0;
            bool stop = false;

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
                    pagePosts.Add(post);

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

        public async Task<BlogComment[]> GetCommentsAsync(string postFileName)
        {
            var commentData = await _commentRepository.GetByPostAsync(postFileName);

            _logger.LogDebug("Found {count} comments for {post}", commentData.Length, postFileName);

            var results = _commentBuilder.BuildTree(commentData);

            return results;
        }

        public async Task<long?> CreateCommentAsync(string postFileName, long? parentId, string author, string email, string markdown)
        {
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

            return id;
        }

        public async Task DeleteCommentAsync(long id)
        {
            await _commentRepository.MarkDeletedAsync(id);

            _logger.LogDebug("Marked comment {id} as deleted (if it exists)");
        }
    }
}
