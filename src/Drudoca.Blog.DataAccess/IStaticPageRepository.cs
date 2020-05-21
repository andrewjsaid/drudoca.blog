using System.Collections.Generic;
using System.Threading.Tasks;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess
{
    public interface IStaticPageRepository
    {
        IAsyncEnumerable<StaticPageData> GetAllAsync();
        Task<StaticPageData?> GetByUriSegmentAsync(string uriSegment);
        Task<bool> HasPageAsync(string uriSegment);
    }
}
