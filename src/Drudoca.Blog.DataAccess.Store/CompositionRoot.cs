using Microsoft.Extensions.DependencyInjection;

namespace Drudoca.Blog.DataAccess.Store
{
    public class CompositionRoot
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTransient<IBlogStore, CachedBlogStore>()
                .AddTransient<IBlogRepository, StoreBlogRepository>();
        }
    }
}
