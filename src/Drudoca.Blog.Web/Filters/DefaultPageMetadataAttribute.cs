﻿using Drudoca.Blog.Domain;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Drudoca.Blog.Web.Filters;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class DefaultPageMetadataAttribute : ActionFilterAttribute, IAsyncPageFilter
{
    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        if (context.Result is null or PageResult)
        { 
            AddPageMetadata(context);
        }

        await next();
    }

    private static void AddPageMetadata(PageHandlerExecutingContext context)
    {
        var seoOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<SeoOptions>>().Value;

        var pageMetadata = new PageMetadata(null, seoOptions.MetaDescription, seoOptions.MetaKeywords);

        var model = (PageModel)context.HandlerInstance;
            
        model.ViewData["PageMetadata"] = pageMetadata;
    }
}