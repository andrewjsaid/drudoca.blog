using Drudoca.Blog.Models;

namespace Drudoca.Blog.ViewModels
{
    public interface IViewModelBuilder
    {
        BlogPostViewModel Build(BlogPost blogPost);
    }
}
