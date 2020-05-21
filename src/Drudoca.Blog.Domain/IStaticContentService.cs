using System.Threading.Tasks;

namespace Drudoca.Blog.Domain
{
    public interface IStaticContentService
    {
        Task<StaticPageMenuItem[]> GetStaticPageMenuAsync();
        Task<StaticPage[]> GetAllPagesAsync();
        Task<StaticPage?> GetPageAsync(string uriSegment);
        Task<bool> HasPageAsync(string uriSegment);
    }
}
