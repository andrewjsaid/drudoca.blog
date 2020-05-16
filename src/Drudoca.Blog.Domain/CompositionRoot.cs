using Microsoft.Extensions.DependencyInjection;

namespace Drudoca.Blog.Domain
{
    public class CompositionRoot
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTransient<IPostBuilder, PostBuilder>()
                .AddTransient<ICommentBuilder, CommentBuilder>()
                .AddTransient<IBlogService, BlogService>()
                .AddScoped<IMarkdownParser, MarkdownParser>();
        }
    }
}
