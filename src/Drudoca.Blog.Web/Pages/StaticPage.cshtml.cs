using System.Threading.Tasks;
using Drudoca.Blog.Domain;
using Drudoca.Blog.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drudoca.Blog.Web.Pages
{
    [LayoutViewData]
    public class StaticPageModel : PageModel
    {
        private readonly IStaticContentService _staticContentService;

        public StaticPageModel(IStaticContentService staticContentService)
        {
            _staticContentService = staticContentService;
        }

        [BindProperty(SupportsGet = true), FromRoute]
        public string UriSegment { get; set; } = default!;

        public StaticPage? SPage { get; set; }

        public async Task OnGetAsync()
        {
            SPage = await _staticContentService.GetPageAsync(UriSegment);
        }
    }
}