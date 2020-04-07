using System;
using Microsoft.Extensions.DependencyInjection;

namespace Drudoca.Blog.Domain
{
    public class CompositionRoot
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTransient<IBlogManager, BlogManager>();
        }
    }
}
