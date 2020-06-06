using Drudoca.Blog.Domain.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace Drudoca.Blog.Domain
{
    public class CompositionRoot
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTransient<ICommentBuilder, CommentBuilder>()
                .AddTransient<IPageMetadataBuilder, PageMetadataBuilder>()
                .AddTransient<IPostBuilder, PostBuilder>()
                .AddTransient<IStaticPageBuilder, StaticPageBuilder>()
                .AddTransient<IStaticPageMenuItemBuilder, StaticPageMenuItemBuilder>()

                .AddTransient<IBlogService, BlogService>()
                .AddTransient<IStaticContentService, StaticContentService>()

                .AddTransient<IEmailService, EmailService>()
                .AddTransient<ITemplateEngine, TemplateEngine>()
                .AddTransient<INotificationBuilder, NotificationBuilder>()

                .AddScoped<IMarkdownParser, MarkdownParser>();
        }
    }
}
