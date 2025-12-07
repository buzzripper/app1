using Dyvenix.App1.Portal.Server.Interfaces;
using Dyvenix.System.Shared.DTOs;

namespace Dyvenix.App1.Portal.Server.Controllers;

[ApiController]
[Asp.Versioning.ApiVersion("1.0")] // Fully qualified to avoid ambiguity
[Route("api/portal/v{version:apiVersion}/[controller]")]
[Route("api/portal/[controller]")] // Fallback route without version
public class SystemController : ControllerBase
{
	private readonly ISystemService _systemService;

	public SystemController(ISystemService systemService)
	{
		_systemService = systemService;
	}

	[HttpGet("[action]")]
	[AllowAnonymous]
	public async Task<ActionResult<object>> Ping()
	{
		return Ok(new PingResult(PortalConstants.ModuleId, ControllerContext.ActionDescriptor.ControllerName));
	}

	[HttpGet("[action]")]
	[AllowAnonymous]
	public async Task<ActionResult<object>> Health()
	{
		var health = await _systemService.Health();
		return Ok(health);
	}
}
