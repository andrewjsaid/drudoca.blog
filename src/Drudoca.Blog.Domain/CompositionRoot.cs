using Drudoca.Blog.Domain.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace Drudoca.Blog.Domain;

public static class CompositionRoot
{
    public static IServiceCollection ConfigureDomainServices(this IServiceCollection services)
    {
        services.AddOptions<BlogOptions>().BindConfiguration("Blog").ValidateOnStart();
        services.AddOptions<EmailOptions>().BindConfiguration("Email").ValidateOnStart();
        services.AddOptions<NotificationOptions>().BindConfiguration("Notifications").ValidateOnStart();

        services.AddTransient<ICommentBuilder, CommentBuilder>();
        services.AddTransient<IPageMetadataBuilder, PageMetadataBuilder>();
        services.AddTransient<IPostBuilder, PostBuilder>();
        services.AddTransient<IStaticPageBuilder, StaticPageBuilder>();
        services.AddTransient<IStaticPageMenuItemBuilder, StaticPageMenuItemBuilder>();

        services.AddTransient<IBlogService, BlogService>();
        services.AddTransient<IStaticContentService, StaticContentService>();

        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<ITemplateEngine, TemplateEngine>();
        services.AddTransient<INotificationBuilder, NotificationBuilder>();

        services.AddScoped<IMarkdownParser, MarkdownParser>();

        return services;
    }
}