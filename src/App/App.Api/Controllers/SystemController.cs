using App1.App.Shared.Interfaces;
using Dyvenix.App1.App.Api.Services;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Common.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App1.App.Api.Controllers;

[ApiController]
[Authorize]
[ServiceFilter(typeof(ApiExceptionFilter<AppSystemService>))]
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
		//throw new ValidationException("Controller ex!!!!!!!!!");
		//return Ok(await CallServiceAsync(_systemService.Health));
		var healthStatus = await _systemService.Health();

		//var a = new ApiResponse<AppHealthStatus>
		//{
		//	Data = healthStatus
		//};

		return Ok(healthStatus);
	}
}
