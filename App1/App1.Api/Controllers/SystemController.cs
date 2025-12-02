using Dyvenix.App1.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dyvenix.App1.Api.Controllers;

[ApiController]
[Asp.Versioning.ApiVersion("1.0")]  // Fully qualified to avoid ambiguity
[Route("api/app1/v{version:apiVersion}/[controller]")]
[Route("api/app1/[controller]")] // Fallback route without version
public class SystemController : ControllerBase
{
	private readonly IApp1SystemService _systemService;

	public SystemController(IApp1SystemService systemService)
	{
		_systemService = systemService;
	}

	[HttpGet("alive")]
	[AllowAnonymous]
	public async Task<ActionResult<object>> Alive()
	{
		var health = await _systemService.Alive();
		return Ok(health);
	}

	[HttpGet("health")]
	[AllowAnonymous]
	public async Task<ActionResult<object>> Health()
	{
		var health = await _systemService.Health();
		return Ok(health);
	}
}
