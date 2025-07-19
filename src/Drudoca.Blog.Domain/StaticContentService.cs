using Drudoca.Blog.DataAccess;

namespace Drudoca.Blog.Domain;

internal class StaticContentService(
    IStaticPageRepository repository,
    IStaticPageBuilder builder,
    IStaticPageMenuItemBuilder menuItemBuilder)
    : IStaticContentService
{
    public async Task<StaticPageMenuItem[]> GetStaticPageMenuAsync()
    {
        var results = new List<StaticPageMenuItem>();

        await foreach (var staticPageData in repository.GetAllAsync())
        {
            var menuItem = menuItemBuilder.Build(staticPageData);
            if (menuItem != null)
            {
                results.Add(menuItem);
            }
        }

        return results.ToArray();
    }

    public async Task<StaticPage[]> GetAllPagesAsync()
    {
        var results = new List<StaticPage>();

        await foreach (var staticPageData in repository.GetAllAsync())
        {
            var page = builder.Build(staticPageData);
            results.Add(page);
        }

        return results.ToArray();
    }

    public async Task<StaticPage?> GetPageAsync(string uriSegment)
    {
        var page = await repository.GetByUriSegmentAsync(uriSegment);
        if (page == null)
        {
            return null;
        }

        var result = builder.Build(page);
        return result;
    }

    public Task<bool> HasPageAsync(string uriSegment) => repository.HasPageAsync(uriSegment);
}