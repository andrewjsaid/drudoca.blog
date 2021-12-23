using System.Security.Claims;
using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.Web.Extensions
{
    public class SiteAdministratorClaimsTransformation : IClaimsTransformation
	{
		private readonly SiteOptions _siteOptions;
		private readonly ILogger _logger;

		public SiteAdministratorClaimsTransformation(
			SiteOptions siteOptions,
			ILogger<SiteAdministratorClaimsTransformation> logger)
		{
			_siteOptions = siteOptions;
			_logger = logger;
		}

		public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
			if (principal.Identity?.IsAuthenticated == true)
			{
				if (IsAdministrator(principal))
				{
					AddClaim(principal);
				}
			}
			return Task.FromResult(principal);
		}

		private bool IsAdministrator(ClaimsPrincipal user)
		{
			var administrators = _siteOptions.Administrators;
			if (administrators == null || administrators.Length == 0)
				return false;

			foreach (var administrator in administrators)
			{
				if (user.HasClaim(ClaimTypes.Email, administrator))
				{
					return true;
				}
			}

			return false;
		}

		private void AddClaim(ClaimsPrincipal user)
		{
			if (user.HasClaim(ClaimTypes.Role, "SiteAdministrator"))
				return; // Should be idempotent

			_logger.LogInformation("Adding administrator claims for administrator {name}", user.Identity?.Name);

			var identity = new ClaimsIdentity("Drudoca");
			identity.AddClaim(new Claim(ClaimTypes.Role, "SiteAdministrator"));
			user.AddIdentity(identity);
		}
	}
}
