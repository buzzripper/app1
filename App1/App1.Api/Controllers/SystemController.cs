using Dyvenix.App1.Shared;
using Dyvenix.App1.Shared.Interfaces;
using Dyvenix.System.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dyvenix.App1.Api.Controllers;

[ApiController]
[Asp.Versioning.ApiVersion("1.0")]  // Fully qualified to avoid ambiguity
[Route("api/app1/v{version:apiVersion}/[controller]")]
[Route("api/app1/[controller]")] // Fallback route without version
public class SystemController : ControllerBase
{
	private readonly ISystemService _systemService;

	public SystemController(ISystemService systemService)
	{
		_systemService = systemService;
	}

	[HttpGet("alive")]
	[AllowAnonymous]
	public async Task<ActionResult<object>> Ping()
	{
		return Ok(new PingResult(App1Constants.ModuleId, ControllerContext.ActionDescriptor.ControllerName));
	}

	[HttpGet("health")]
	[AllowAnonymous]
	public async Task<ActionResult<object>> Health()
	{
		var health = await _systemService.Health();
		return Ok(health);
	}
}
