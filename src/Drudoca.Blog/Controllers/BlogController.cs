using System;
using System.Threading.Tasks;
using Drudoca.Blog.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger _logger;
        private readonly IBlogPostRepository _blogPostRepository;

        public BlogController(
            ILogger<BlogController> logger,
            IBlogPostRepository blogPostRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _blogPostRepository = blogPostRepository ?? throw new ArgumentNullException(nameof(blogPostRepository));
        }

        public async Task<IActionResult> Post(int year, int month, string slug)
        {
            _logger.LogDebug("Requested blog post {year}/{month}/{slug}", year, month, slug);
            var blogPost = await _blogPostRepository.GetBlogPost(year, month, slug);

            if(blogPost == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(blogPost);
        }

        public async Task<IActionResult> Page(int id)
        {
            _logger.LogDebug("Requested page {page}", id);
            var blogPosts = await _blogPostRepository.GetBlogPostsAsync(id);
            return View(blogPosts);

        }
    }
}
