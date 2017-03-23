using Drudoca.Blog.Models;
using Drudoca.Blog.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Drudoca.Blog.DataAccess
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private const int GetBlogPostLogEvent = 1;
        private const int GetBlogPostsLogEvent = 2;

        private readonly ILogger _logger;
        private readonly IOptions<FormattingOptions> _formatOptions;

        public BlogPostRepository(
            ILogger<BlogPostRepository> logger,
            IOptions<FormattingOptions> formatOptions)
        {
            _logger = logger;
            _formatOptions = formatOptions ?? throw new ArgumentNullException(nameof(formatOptions));
        }

        public ValueTask<BlogPost> GetBlogPost(string headline)
        {
            _logger.LogDebug(GetBlogPostLogEvent, "Retrieving post with heading {HEADLINE}", headline);

            var result = new BlogPost
            {
                Title = "Blog Post 1",
                Markdown = "This is the first post",
                PublishedDate = new DateTime(2017, 03, 21)
            };
            return new ValueTask<BlogPost>(result);
        }

        public ValueTask<BlogPost[]> GetBlogPosts(int pageNum)
        {
            var formatOptions = _formatOptions.Value;

            _logger.LogDebug(GetBlogPostsLogEvent, "Retrieving {PAGESIZE} posts on page {PAGENUM}", formatOptions.PageSize, pageNum);

            var result = new[]
            {
                new BlogPost{
                    Title = "Blog Post 1",
                    Markdown="This is the first post",
                    PublishedDate = new DateTime(2017, 03, 21)
                },
                new BlogPost{
                    Title = "Blog Post 2",
                    Markdown="This is the second post",
                    PublishedDate = new DateTime(2017, 03, 22)
                }
            };

            return new ValueTask<BlogPost[]>(result);
        }
    }
}