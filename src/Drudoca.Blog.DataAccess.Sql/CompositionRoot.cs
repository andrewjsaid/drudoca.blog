using Microsoft.Extensions.DependencyInjection;

namespace Drudoca.Blog.DataAccess.Sql;

public static class CompositionRoot
{
    public static IServiceCollection ConfigureDataAccessSqlServices(this IServiceCollection services)
    {
        services.AddSingleton<MigrationManager>();
        services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();
        services.AddTransient<ICommentRepository, SqlCommentRepository>();

        services.AddOptions<DatabaseOptions>().BindConfiguration("Database").ValidateOnStart();
        return services;
    }
}