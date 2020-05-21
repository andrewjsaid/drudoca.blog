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
                .AddTransient<IStaticPageBuilder, StaticPageBuilder>()
                .AddTransient<IStaticPageMenuItemBuilder, StaticPageMenuItemBuilder>()

                .AddTransient<IBlogService, BlogService>()
                .AddTransient<IStaticContentService, StaticContentService>()

                .AddScoped<IMarkdownParser, MarkdownParser>();
        }
    }
}
