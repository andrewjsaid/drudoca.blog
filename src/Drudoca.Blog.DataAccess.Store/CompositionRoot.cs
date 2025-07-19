using Drudoca.Blog.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Drudoca.Blog.DataAccess.Store;

public static class CompositionRoot
{
    public static IServiceCollection ConfigureDataAccessStoreServices(this IServiceCollection services)
    {
        services.AddOptions<StoreOptions>().BindConfiguration("Store").ValidateOnStart();

        services.AddTransient<IMarkdownDirectoryReader, MarkdownDirectoryReader>();

        services.AddTransient<IMarkdownFileConverter<PostData>, BlogPostFileConverter>();
        services.AddTransient<IMarkdownStore<PostData>, CachedMarkdownStore<PostData>>();
        services.AddTransient<IPostRepository, FilePostRepository>();

        services.AddTransient<IMarkdownFileConverter<StaticPageData>, StaticPageFileConverter>();
        services.AddTransient<IMarkdownStore<StaticPageData>, CachedMarkdownStore<StaticPageData>>();
        services.AddTransient<IStaticPageRepository, StaticPageRepository>();

        services.AddTransient<IMarkdownFileConverter<EmailTemplateData>, EmailTemplateFileConverter>();
        services.AddTransient<IMarkdownStore<EmailTemplateData>, CachedMarkdownStore<EmailTemplateData>>();
        services.AddTransient<IEmailTemplateRepository, EmailTemplateRepository>();

        return services;
    }
}