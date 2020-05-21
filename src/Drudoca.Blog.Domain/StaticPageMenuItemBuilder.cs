using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain
{
    internal class StaticPageMenuItemBuilder : IStaticPageMenuItemBuilder
    {
        public StaticPageMenuItem? Build(StaticPageData data)
        {
            if (data.MenuIcon == null && string.IsNullOrEmpty(data.MenuText))
            {
                return null;
            }

            var result = new StaticPageMenuItem(
                data.MenuSequence ?? default,
                data.MenuText ?? string.Empty,
                data.MenuIcon,
                data.UriSegment);

            return result;
        }
    }
}