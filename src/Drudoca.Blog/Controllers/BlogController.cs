using Drudoca.Blog.DataAccess;
using Drudoca.Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Drudoca.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger _logger;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IViewModelBuilder _viewModelBuilder;

        public BlogController(
            ILogger<BlogController> logger,
            IBlogPostRepository blogPostRepository,
            IViewModelBuilder viewModelBuilder)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _blogPostRepository = blogPostRepository ?? throw new ArgumentNullException(nameof(blogPostRepository));
            _viewModelBuilder = viewModelBuilder ?? throw new ArgumentNullException(nameof(viewModelBuilder));
        }

        public async Task<IActionResult> Post(string id)
        {
            _logger.LogDebug("Requested blog post {slug}", id);
            var blogPost = await _blogPostRepository.GetBlogPost(id);

            if(blogPost == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var viewModel = _viewModelBuilder.Build(blogPost);
            return View(viewModel);
        }
    }
}
