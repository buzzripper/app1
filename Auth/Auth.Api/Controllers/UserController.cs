using App1.Auth.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace App1.Auth.Api.Controllers;

/// <summary>
/// Provides user information based on JWT claims.
/// This endpoint is called by Angular to check authentication status and get user profile.
/// </summary>
[Route("api/auth/[controller]")]
[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
public class UserController : ControllerBase
{
	/// <summary>
	/// Returns the current user's profile based on JWT claims.
	/// Returns anonymous user info if not authenticated.
	/// </summary>
	[HttpGet]
	[AllowAnonymous]
	public IActionResult GetCurrentUser() => Ok(CreateUserInfo(User));

	private static UserInfo CreateUserInfo(ClaimsPrincipal claimsPrincipal)
	{
		if (claimsPrincipal?.Identity?.IsAuthenticated != true)
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

		if (claimsPrincipal.Claims?.Any() == true)
		{
			userInfo.Claims = claimsPrincipal.Claims
				.Select(c => new ClaimValue(c.Type, c.Value))
				.ToList();
		}

		return userInfo;
	}
}
