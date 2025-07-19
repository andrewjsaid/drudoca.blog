using Drudoca.Blog.Domain;
using Drudoca.Blog.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drudoca.Blog.Web.Pages;

[LayoutModel]
public class StaticPageModel(IStaticContentService staticContentService) : PageModel
{
    [BindProperty(SupportsGet = true), FromRoute]
    public string UriSegment { get; set; } = default!;

    public StaticPage? SPage { get; set; }

    public async Task OnGetAsync()
    {
        SPage = await staticContentService.GetPageAsync(UriSegment);
    }
}