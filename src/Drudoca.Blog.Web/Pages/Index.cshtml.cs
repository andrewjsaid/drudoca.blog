using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Drudoca.Blog.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;


namespace Drudoca.Blog.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBlogManager _blogManager;
        private readonly IOptions<BlogOptions> _blogOptions;

        public IndexModel(
            IBlogManager blogManager,
            IOptions<BlogOptions> blogOptions)
        {
            _blogManager = blogManager;
            _blogOptions = blogOptions;
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