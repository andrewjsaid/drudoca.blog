using Drudoca.Blog.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Drudoca.Blog.DataAccess.Store
{
    public class CompositionRoot
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTransient<IMarkdownFileConverter<PostData>, BlogPostFileConverter>()
                .AddTransient<IMarkdownStore<PostData>, CachedMarkdownStore<PostData>>()

                .AddTransient<IPostRepository, FilePostRepository>()
                .AddTransient<IMarkdownDirectoryReader, MarkdownDirectoryReader>();
        }
    }
}
