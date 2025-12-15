using App1.Auth.Api.Filters;
using App1.Auth.Api.Services;
using App1.Auth.Shared.Interfaces;
using App1.System.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App1.Auth.Api.Controllers;

[ApiController]
[ServiceFilter(typeof(AuthExceptionFilter<AuthSystemService>))]
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

	[HttpGet("[action]")]
	[AllowAnonymous]
	public async Task<ActionResult<object>> Ping()
	{
		return Ok(new PingResult(AuthConstants.ModuleId, ControllerContext.ActionDescriptor.ControllerName));
	}

	[HttpGet("[action]")]
	[AllowAnonymous]
	public async Task<ActionResult<object>> Health()
	{
		//throw new ValidationException("Controller ex!!!!!!!!!");

		var health = await _systemService.Health();
		return Ok(health);
	}
}
