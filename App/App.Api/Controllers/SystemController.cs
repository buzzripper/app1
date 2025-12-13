using Dyvenix.App.Shared;
using Dyvenix.App.Shared.DTOs;
using Dyvenix.App.Shared.Interfaces;
using Dyvenix.System.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App.Api.Controllers;

[ApiController]
[Asp.Versioning.ApiVersion("1.0")]  // Fully qualified to avoid ambiguity
[Route("api/app/v{version:apiVersion}/[controller]")]
[Route("api/app/[controller]")] // Fallback route without version
public class SystemController : ControllerBase
{
	private readonly IAppSystemService _systemService;

	public SystemController(IAppSystemService systemService, ILogger<SystemController> logger)
	{
		_systemService = systemService;
	}

	[HttpGet("ping")]
	[AllowAnonymous]
	public async Task<ActionResult<object>> Ping()
	{
		return Ok(new PingResult(AppConstants.ModuleId, ControllerContext.ActionDescriptor.ControllerName));
	}

	[HttpGet("health")]
	[AllowAnonymous]
	public async Task<IActionResult> Health()
	{
		//return Ok(await CallServiceAsync(_systemService.Health));
		var healthStatus = await _systemService.Health();

		//var a = new ApiResponse<AppHealthStatus>
		//{
		//	Data = healthStatus
		//};

		return Ok(healthStatus);
	}
}
