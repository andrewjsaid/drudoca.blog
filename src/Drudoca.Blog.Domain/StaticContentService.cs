using System.Collections.Generic;
using System.Threading.Tasks;
using Drudoca.Blog.DataAccess;

namespace Drudoca.Blog.Domain
{
    internal class StaticContentService : IStaticContentService
    {
        private readonly IStaticPageRepository _repository;
        private readonly IStaticPageBuilder _builder;
        private readonly IStaticPageMenuItemBuilder _menuItemBuilder;

        public StaticContentService(
            IStaticPageRepository repository,
            IStaticPageBuilder builder,
            IStaticPageMenuItemBuilder menuItemBuilder)
        {
            _repository = repository;
            _builder = builder;
            _menuItemBuilder = menuItemBuilder;
        }

        public async Task<StaticPageMenuItem[]> GetStaticPageMenuAsync()
        {
            var results = new List<StaticPageMenuItem>();

            await foreach (var staticPageData in _repository.GetAllAsync())
            {
                var menuItem = _menuItemBuilder.Build(staticPageData);
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

            await foreach (var staticPageData in _repository.GetAllAsync())
            {
                var page = _builder.Build(staticPageData);
                results.Add(page);
            }

            return results.ToArray();
        }

        public async Task<StaticPage?> GetPageAsync(string uriSegment)
        {
            var page = await _repository.GetByUriSegmentAsync(uriSegment);
            if (page == null)
            {
                return null;
            }

            var result = _builder.Build(page);
            return result;
        }
    }
}
