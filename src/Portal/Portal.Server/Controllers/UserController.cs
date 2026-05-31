using Dyvenix.App1.Portal.Server.Models;

namespace Dyvenix.App1.Portal.Server.Controllers;

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
		var tenantId = Guid.TryParse(tenantIdStr, out var parsed) ? parsed : (Guid?)null;

		return new LoggedInUserDto
		{
			IsAuthenticated = true,
			UserId = principal.FindFirstValue("sub") ?? string.Empty,
			Name = principal.FindFirstValue("name") ?? string.Empty,
			Email = principal.FindFirstValue("email") ?? string.Empty,
			TenantId = tenantId
		};
	}
}
