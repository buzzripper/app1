using App1.Auth.Api.Filters;
using App1.Auth.Api.Services;
using App1.Auth.Shared.Interfaces;
using App1.System.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App1.Auth.Api.Controllers;

[ApiController]
[ServiceFilter(typeof(AuthExceptionFilter<AuthSystemService>))]
[Asp.Versioning.ApiVersion("1.0")]
[Route("api/auth/v{version:apiVersion}/[controller]")]
public class SystemController : ControllerBase
{
	private readonly IAuthSystemService _systemService;

	public SystemController(IAuthSystemService systemService)
	{
		_systemService = systemService;
	}

	[HttpGet("[action]")]
	public async Task<ActionResult<object>> Ping()
	{
		return Ok(new PingResult(AuthConstants.ModuleId, ControllerContext.ActionDescriptor.ControllerName));
	}

	[HttpGet("[action]")]
	public async Task<ActionResult<object>> Health()
	{
		//throw new ValidationException("Controller ex!!!!!!!!!");

		var health = await _systemService.Health();
		return Ok(health);
	}
}
