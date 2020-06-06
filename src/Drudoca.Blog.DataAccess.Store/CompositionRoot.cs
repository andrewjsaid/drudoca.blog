using Drudoca.Blog.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Drudoca.Blog.DataAccess.Store
{
    public class CompositionRoot
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTransient<IMarkdownDirectoryReader, MarkdownDirectoryReader>()

                .AddTransient<IMarkdownFileConverter<PostData>, BlogPostFileConverter>()
                .AddTransient<IMarkdownStore<PostData>, CachedMarkdownStore<PostData>>()
                .AddTransient<IPostRepository, FilePostRepository>()

                .AddTransient<IMarkdownFileConverter<StaticPageData>, StaticPageFileConverter>()
                .AddTransient<IMarkdownStore<StaticPageData>, CachedMarkdownStore<StaticPageData>>()
                .AddTransient<IStaticPageRepository, StaticPageRepository>()

                .AddTransient<IMarkdownFileConverter<EmailTemplateData>, EmailTemplateFileConverter>()
                .AddTransient<IMarkdownStore<EmailTemplateData>, CachedMarkdownStore<EmailTemplateData>>()
                .AddTransient<IEmailTemplateRepository, EmailTemplateRepository>();
        }
    }
}
