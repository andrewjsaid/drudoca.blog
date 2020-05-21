using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Drudoca.Blog.Domain;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drudoca.Blog.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class LayoutViewDataAttribute : ActionFilterAttribute, IAsyncPageFilter
    {
        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (context.Result == null || context.Result is PageResult)
            {
                await AddLayoutViewDataAsync(context);
            }

            await next();
        }

        private static async Task AddLayoutViewDataAsync(PageHandlerExecutingContext context)
        {
            var staticContentService = (IStaticContentService)context.HttpContext.RequestServices.GetService(typeof(IStaticContentService));
            Debug.Assert(staticContentService != null);

            var model = (PageModel)context.HandlerInstance;

            var menu = await staticContentService.GetStaticPageMenuAsync();
            model.ViewData["StaticPageMenu"] = menu;
        }
    }
}
