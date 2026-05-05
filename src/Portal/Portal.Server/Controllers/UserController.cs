using App1.App1.Portal.Server.Models;

namespace App1.App1.Portal.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	[HttpGet]
	[AllowAnonymous]
	public IActionResult GetCurrentUser() => Ok(CreateUserInfo(User));

	private static UserInfo CreateUserInfo(ClaimsPrincipal claimsPrincipal)
	{
		if (claimsPrincipal == null || claimsPrincipal.Identity == null || !claimsPrincipal.Identity.IsAuthenticated)
			return UserInfo.Anonymous;

		var userInfo = new UserInfo { IsAuthenticated = true };

		if (claimsPrincipal.Identity is ClaimsIdentity claimsIdentity)
		{
			userInfo.NameClaimType = claimsIdentity.NameClaimType;
			userInfo.RoleClaimType = claimsIdentity.RoleClaimType;
		}
		else
		{
			userInfo.NameClaimType = ClaimTypes.Name;
			userInfo.RoleClaimType = ClaimTypes.Role;
		}

		if (claimsPrincipal.Claims?.Any() ?? false)
			userInfo.Claims = claimsPrincipal.Claims.Select(u => new ClaimValue(u.Type, u.Value)).ToList();

		return userInfo;
	}
}
