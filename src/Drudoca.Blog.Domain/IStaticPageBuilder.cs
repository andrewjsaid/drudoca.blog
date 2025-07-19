using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain;

internal interface IStaticPageBuilder
{
    StaticPage Build(StaticPageData data);
}