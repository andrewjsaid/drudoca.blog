using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drudoca.Blog.Data;
using Drudoca.Blog.DataAccess;
using Markdig;

namespace Drudoca.Blog.Domain
{
    internal class BlogManager : IBlogManager
    {
        private readonly IBlogRepository _repository;

        public BlogManager(
            IBlogRepository repository)
        {
            _repository = repository;
        }

        public async Task<BlogPage> GetPageAsync(int pageSize, int pageNum)
        {
            if (pageNum <= 0) throw new ArgumentOutOfRangeException(nameof(pageNum), "pageNum must not be less than 0");

            int skip = pageSize * (pageNum - 1);

            var pagePosts = new List<BlogPost>(pageSize);

            await foreach (var postData in _repository.GetAllPostsAsync())
            {
                // Keep skipping
                if (skip > 0)
                {
                    skip--;
                    continue;
                }

                // Check if we filled the page
                if (pagePosts.Count == pageSize)
                {
                    break;
                }

                var post = CreatePost(postData);
                pagePosts.Add(post);
            }

            var count = await _repository.CountBlogPostsAsync();
            var pageCount = (int)Math.Ceiling(count / (double)pageSize);

            var result = new BlogPage(pageNum, pageSize, pageCount, pagePosts.ToArray());
            return result;
        }

        public async Task<BlogPost?> GetPostAsync(DateTime published, string slug)
        {
            await foreach (var post in _repository.GetPostsByDateAsync(published))
            {
                var postSlug = UrlSlug.Slugify(post.Title);
                if (string.Equals(postSlug, slug, StringComparison.OrdinalIgnoreCase))
                {
                    return CreatePost(post);
                }
            }
            return null;
        }

        private BlogPost CreatePost(BlogPostData data)
        {
            var result = new BlogPost(
                data.Title,
                data.Author,
                data.PublishedOn,
                data.IsPublished,
                data.Markdown,
                UrlSlug.Slugify(data.Title),
                Markdown.ToHtml(data.Markdown)
            );
            return result;
        }
    }
}
