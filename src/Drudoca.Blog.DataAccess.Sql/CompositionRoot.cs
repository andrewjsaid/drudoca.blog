using Microsoft.Extensions.DependencyInjection;

namespace Drudoca.Blog.DataAccess.Sql
{
    public class CompositionRoot
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<MigrationManager>()
                .AddTransient<IDbConnectionFactory, DbConnectionFactory>()
                .AddTransient<ICommentRepository, SqlCommentRepository>();
        }
    }
}
