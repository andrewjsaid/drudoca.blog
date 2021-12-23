using System.Diagnostics;
using System.Threading.Tasks;
using Drudoca.Blog.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Drudoca.Blog.Web.Routing
{
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
}
