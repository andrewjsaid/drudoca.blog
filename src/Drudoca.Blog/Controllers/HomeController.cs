using Drudoca.Blog;
using Drudoca.Blog.DataAccess;
using Drudoca.Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Drudoca.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IViewModelBuilder _viewModelBuilder;

        public HomeController(
            ILogger<HomeController> logger,
            IBlogPostRepository blogPostRepository,
            IViewModelBuilder viewModelBuilder)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _blogPostRepository = blogPostRepository ?? throw new ArgumentNullException(nameof(blogPostRepository));
            _viewModelBuilder = viewModelBuilder ?? throw new ArgumentNullException(nameof(viewModelBuilder));
        }

        public IActionResult Index()
        {
            return RedirectToAction("page", "blog", new { id = 1 });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
