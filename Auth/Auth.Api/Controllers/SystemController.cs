using Dyvenix.Auth.Shared;
using Dyvenix.Auth.Shared.Interfaces;
using Dyvenix.System.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
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
		var health = await _systemService.Health();
		return Ok(health);
	}
}
