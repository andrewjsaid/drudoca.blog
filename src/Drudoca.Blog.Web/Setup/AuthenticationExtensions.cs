using Microsoft.AspNetCore.Authentication.Cookies;

namespace Drudoca.Blog.Web.Setup;

internal static class AuthenticationExtensions
{

    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie()
            .AddGoogle(options =>
            {
                var googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
                options.ClientId = googleAuthNSection["ClientId"]!;
                options.ClientSecret = googleAuthNSection["ClientSecret"]!;
            });
    }

}
