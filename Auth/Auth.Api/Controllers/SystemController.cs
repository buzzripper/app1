using Dyvenix.Auth.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dyvenix.Auth.Api.Controllers;

[ApiController]
[Asp.Versioning.ApiVersion("1.0")] // Fully qualified to avoid ambiguity
[Route("api/auth/v{version:apiVersion}/[controller]")]
[Route("api/auth/[controller]")] // Fallback route without version
public class SystemController : ControllerBase
{
	private readonly IAuthSystemService _systemService;

	public SystemController(IAuthSystemService systemService)
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
