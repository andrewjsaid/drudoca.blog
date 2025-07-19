using Microsoft.AspNetCore.Authentication.Google;

namespace Drudoca.Blog.Web.Api;

public static class AuthEndpoints
{
    public static void MapAuthApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("google", (string? redirect = null) =>
            Results.Challenge(
                new GoogleChallengeProperties { RedirectUri = redirect },
                [GoogleDefaults.AuthenticationScheme]));
    }
}
