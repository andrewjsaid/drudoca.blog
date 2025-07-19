using System.Diagnostics;
using Drudoca.Blog.Domain;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Drudoca.Blog.Web.Routing;

public class StaticPageRouteValueTransformer : DynamicRouteValueTransformer
{
    public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        var staticContentService = (IStaticContentService)httpContext.RequestServices.GetRequiredService(typeof(IStaticContentService));

        var uriSegment = (string?)values["uriSegment"];
        Debug.Assert(!string.IsNullOrEmpty(uriSegment));

        var isPage = await staticContentService.HasPageAsync(uriSegment);
        if (isPage)
        {
            values["page"] = "/StaticPage";
        }

        return values;
    }
}