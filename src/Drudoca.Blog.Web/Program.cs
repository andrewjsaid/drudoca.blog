using Drudoca.Blog.DataAccess.Sql;
using Drudoca.Blog.DataAccess.Store;
using Drudoca.Blog.Domain;
using Drudoca.Blog.Web;
using Drudoca.Blog.Web.Api;
using Drudoca.Blog.Web.Extensions;
using Drudoca.Blog.Web.Routing;
using Drudoca.Blog.Web.Setup;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureDataProtection();
builder.ConfigureLettuceEncrypt();
builder.ConfigureAuthentication();

builder.Services.AddRazorPages();

builder.Services.AddOptions<SiteOptions>().BindConfiguration("Site").ValidateOnStart();
builder.Services.AddOptions<SeoOptions>().BindConfiguration("SEO").ValidateOnStart();

builder.Services.AddTransient<IClaimsTransformation, SiteAdministratorClaimsTransformation>();

builder.Services.AddTransient<StaticPageRouteValueTransformer>();

builder.Services.ConfigureDataAccessStoreServices();
builder.Services.ConfigureDataAccessSqlServices();
builder.Services.ConfigureDomainServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Status/Status{0}");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=86400";
    }
});

app.UseStaticFiles(new StaticFileOptions
{
    RequestPath = "/blog-content/images",
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Configuration["Store:BlogPostPath"]!, "images")),
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthApiEndpoints();
app.MapDynamicPageRoute<StaticPageRouteValueTransformer>("{uriSegment}");
app.MapRazorPages();

await app.RunAsync();