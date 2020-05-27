using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Drudoca.Blog.Domain;
using Drudoca.Blog.Web.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drudoca.Blog.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class LayoutModelAttribute : ActionFilterAttribute, IAsyncPageFilter
    {
        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (context.Result == null || context.Result is PageResult)
            { await AddLayoutModelAsync(context);
            }

            await next();
        }

        private static async Task AddLayoutModelAsync(PageHandlerExecutingContext context)
        {
            var siteOptions = (SiteOptions)context.HttpContext.RequestServices.GetService(typeof(SiteOptions));
            var seoOptions = (SeoOptions)context.HttpContext.RequestServices.GetService(typeof(SeoOptions));

            var staticContentService = (IStaticContentService)context.HttpContext.RequestServices.GetService(typeof(IStaticContentService));
            Debug.Assert(staticContentService != null);

            var menu = await staticContentService.GetStaticPageMenuAsync();
            
            var layoutModel = new LayoutModel(
                menu,
                seoOptions,
                siteOptions.GoogleTagManagerClientId,
                siteOptions.FontAwesomeId,
                siteOptions.ThemeColor,
                siteOptions.MetaTags);

            var model = (PageModel)context.HandlerInstance;
            model.ViewData["LayoutModel"] = layoutModel;
        }
    }
}
