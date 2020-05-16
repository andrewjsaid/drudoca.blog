using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace Drudoca.Blog.Web.Controllers
{
    [Route("{controller}")]
    public class AuthController : Controller
    {
        [HttpGet("google")]
        public IActionResult Google(string? redirect = null)
        {
            return Challenge(new GoogleChallengeProperties
            {
                RedirectUri = redirect
            }, GoogleDefaults.AuthenticationScheme);
        }
    }
}