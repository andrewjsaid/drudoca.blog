using Drudoca.Blog.Models;
using System.Threading.Tasks;

namespace Drudoca.Blog.DataAccess
{
    internal interface IBlogPostSource
    {
        Task<BlogPost[]> LoadAsync();
    }
}