//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 1/29/2026 10:34 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.Auth.Api.Filters;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Auth.Api.Services.v1;

namespace Dyvenix.App1.Auth.Api.Controllers.v1;

[ApiController]
[ServiceFilter(typeof(AuthExceptionFilter<PersonService>))]
[Asp.Versioning.ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Route("api/auth/[controller]")]
public class PersonController : ControllerBase
{
	private readonly IPersonService _personService;
	
	#region Create
	
	[HttpPost, Route("[action]")]
	[Authorize("Permission1,Permission2")]
	public async Task<ActionResult> CreatePerson([FromBody] Person personService)
	{
		return Ok(await _personService.CreatePerson(personService));
	}
	
	#endregion
	
	#region Create
	
	[HttpPost, Route("[action]")]
	[Authorize("Permission3")]
	public async Task<ActionResult> DeletePerson(Guid id)
	{
		return Ok(await _personService.DeletePerson(id));
	}
	
	#endregion
}
