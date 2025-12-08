using Dyvenix.App.Shared;
using Dyvenix.App.Shared.Interfaces;
using Dyvenix.System.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dyvenix.App.Api.Controllers;

[ApiController]
[Asp.Versioning.ApiVersion("1.0")]  // Fully qualified to avoid ambiguity
[Route("api/app/v{version:apiVersion}/[controller]")]
[Route("api/app/[controller]")] // Fallback route without version
public class SystemController : ControllerBase
{
	private readonly ISystemService _systemService;

	public SystemController(ISystemService systemService)
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
	public async Task<ActionResult<object>> Health()
	{

		var health = await _systemService.Health();
		return Ok(health);
	}
}
