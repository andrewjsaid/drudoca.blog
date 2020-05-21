using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Drudoca.Blog.Domain;
using Drudoca.Blog.Web.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;


namespace Drudoca.Blog.Web.Pages
{
    [LayoutViewData]
    public class IndexModel : PageModel
    {
        private readonly IBlogService _blogManager;
        private readonly IStaticContentService _staticContentService;
        private readonly IOptions<BlogOptions> _blogOptions;

        public IndexModel(
            IBlogService blogManager,
            IOptions<BlogOptions> blogOptions,
            IStaticContentService staticContentService)
        {
            _blogManager = blogManager;
            _blogOptions = blogOptions;
            _staticContentService = staticContentService;
        }

        public BlogPage? BlogPage { get; set; }

        public async Task OnGet(int pageNum = 1)
        {
            var pageSize = _blogOptions.Value.PageSize;
            var page = await _blogManager.GetPageAsync(pageSize, pageNum);
            BlogPage = page;
        }
    }
}