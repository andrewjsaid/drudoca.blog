using Drudoca.Blog.Models;

namespace Drudoca.Blog.DataAccess
{
    public interface IBlogPostBuilder
    {
        BlogPost Build(string fileName, string fileContents);
    }
}