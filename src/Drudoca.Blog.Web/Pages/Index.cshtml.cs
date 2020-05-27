using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Drudoca.Blog.Domain;
using Drudoca.Blog.Web.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drudoca.Blog.Web.Pages
{
    [LayoutModel]
    [DefaultPageMetadata]
    public class IndexModel : PageModel
    {
        private readonly IBlogService _blogManager;
        private readonly BlogOptions _blogOptions;

        public IndexModel(
            IBlogService blogManager,
            BlogOptions blogOptions
            )
        {
            _blogManager = blogManager;
            _blogOptions = blogOptions;
        }

        public BlogPage? BlogPage { get; set; }

        public async Task OnGet(int pageNum = 1)
        {
            var pageSize = _blogOptions.PageSize;
            var page = await _blogManager.GetPageAsync(pageSize, pageNum);
            BlogPage = page;
        }
    }
}