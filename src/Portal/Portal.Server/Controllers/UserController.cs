using App1.App1.Portal.Server.Models;

namespace App1.App1.Portal.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	[HttpGet]
	[AllowAnonymous]
	public IActionResult GetCurrentUser() => Ok(BuildDto(User));

	private static LoggedInUserDto BuildDto(ClaimsPrincipal principal)
	{
		if (principal?.Identity?.IsAuthenticated != true)
			return LoggedInUserDto.Anonymous;

		var tenantIdStr = principal.FindFirstValue("tenant_id");
		Guid.TryParse(tenantIdStr, out var tenantId);

		return new LoggedInUserDto
		{
			IsAuthenticated = true,
			UserId = principal.FindFirstValue("sub") ?? string.Empty,
			UserName = principal.FindFirstValue("name") ?? string.Empty,
			Email = principal.FindFirstValue("email") ?? string.Empty,
			TenantId = tenantId,
			Roles = principal.FindAll("role").Select(c => c.Value).ToList(),
			Permissions = principal.FindAll("perm").Select(c => c.Value).ToList(),
		};
	}
}
