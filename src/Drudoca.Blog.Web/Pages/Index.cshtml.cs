using Drudoca.Blog.Domain;
using Drudoca.Blog.Web.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drudoca.Blog.Web.Pages;

[LayoutModel]
[DefaultPageMetadata]
public class IndexModel(IBlogService blogService) : PageModel
{
    public BlogPage? BlogPage { get; set; }

    public async Task OnGet(int pageNum = 1)
    {
        var page = await blogService.GetPageAsync(pageNum);
        BlogPage = page;
    }
}