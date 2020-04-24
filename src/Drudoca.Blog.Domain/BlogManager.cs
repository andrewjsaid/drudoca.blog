using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Drudoca.Blog.Data;
using Drudoca.Blog.DataAccess;
using Markdig;
using Microsoft.Extensions.Options;

namespace Drudoca.Blog.Domain
{
    internal class BlogManager : IBlogManager
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IOptions<BlogOptions> _options;

        public BlogManager(
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IOptions<BlogOptions> options)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _options = options;
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
            if (pageNum <= 0) throw new ArgumentOutOfRangeException(nameof(pageNum), "pageNum must not be less than 0");

            int skip = pageSize * (pageNum - 1);

            Debug.Assert(pageSize > 0);
            var pagePosts = new List<BlogPost>(pageSize);

            // We can't use _repository.CountBlogPostsAsync() since
            // it includes "hidden" posts.
            int count = 0;
            bool stop = false;

            var showFuture = _options.Value.ListFuturePosts;

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
                    var post = CreatePost(postData, null);
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
                        var comments = await _commentRepository.GetCommentsForPostAsync(postData.FileName);
                        var post = CreatePost(postData, comments);
                        return post;
                    }
                }
            }

            return null;
        }

        private BlogPost CreatePost(PostData data, CommentData[]? comments)
        {
            var title = UrlSlug.Slugify(data.Title);
            var html = Markdown.ToHtml(data.Markdown);

            string? introHtml = null;

            var mainSectionIndex = data.Markdown.IndexOf("\n[//]: # (Main Section)");
            if (mainSectionIndex > -1)
            {
                var introMarkdown = data.Markdown.Substring(0, mainSectionIndex);
                introHtml = Markdown.ToHtml(introMarkdown);
            }

            var result = new BlogPost(
                data.Title,
                data.Author,
                data.PublishedOn,
                data.IsPublished,
                data.IsListed,
                data.Markdown,
                title,
                html,
                introHtml
            );
            return result;
        }
    }
}
