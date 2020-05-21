using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain
{
    internal interface IStaticPageMenuItemBuilder
    {
        StaticPageMenuItem? Build(StaticPageData data);
    }
}
