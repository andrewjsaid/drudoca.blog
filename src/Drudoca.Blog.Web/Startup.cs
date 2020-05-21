using Drudoca.Blog.Config;
using Drudoca.Blog.Web.Extensions;
using Drudoca.Blog.Web.Routing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Drudoca.Blog.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie()
                    .AddGoogle(options =>
                    {
                        var googleAuthNSection = Configuration.GetSection("Authentication:Google");
                        options.ClientId = googleAuthNSection["ClientId"];
                        options.ClientSecret = googleAuthNSection["ClientSecret"];
                    });

            services.AddRazorPages();

            services.Configure<BlogOptions>(Configuration.GetSection("Blog"));
            services.Configure<DatabaseOptions>(Configuration.GetSection("Database"));
            services.Configure<SiteOptions>(Configuration.GetSection("Site"));
            services.Configure<StoreOptions>(Configuration.GetSection("Store"));

            services.AddTransient(r => r.GetRequiredService<IOptions<BlogOptions>>().Value);
            services.AddTransient(r => r.GetRequiredService<IOptions<DatabaseOptions>>().Value);
            services.AddTransient(r => r.GetRequiredService<IOptions<SiteOptions>>().Value);
            services.AddTransient(r => r.GetRequiredService<IOptions<StoreOptions>>().Value);

            services.AddTransient<IClaimsTransformation, SiteAdmininstratorClaimsTransformation>();

            services.AddTransient<StaticPageRouteValueTransformer>();

            Drudoca.Blog.DataAccess.Store.CompositionRoot.ConfigureServices(services);
            Drudoca.Blog.DataAccess.Sql.CompositionRoot.ConfigureServices(services);
            Drudoca.Blog.Domain.CompositionRoot.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Status/Status{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // As long as we are deploying behind a reverse proxy, we need to allow this. REMOVE this once we support being an external-facing service.
            var forwardedHeadersOptions = new ForwardedHeadersOptions { ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All };
            forwardedHeadersOptions.KnownProxies.Clear();   // Don't know the docker address that nginx will use
            forwardedHeadersOptions.KnownNetworks.Clear();  // Don't know the docker address that nginx will use
            app.UseForwardedHeaders(forwardedHeadersOptions);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            {
                // Remove this next one for ASP.NET Core upgrade - https://github.com/aspnet/AspNetCore/issues/2442
                var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
                provider.Mappings[".webmanifest"] = "application/json";
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot")),
                    RequestPath = "",
                    ContentTypeProvider = provider
                });
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDynamicPageRoute<StaticPageRouteValueTransformer>("{uriSegment}");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
