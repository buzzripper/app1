//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 1/31/2026 7:01 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.Auth.Api.Filters;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Auth.Api.Services.v1;
using Dyvenix.App1.Auth.Shared.DTOs.v1;
using Dyvenix.App1.Auth.Shared.Queries.v1;

namespace Dyvenix.App1.Auth.Api.Controllers.v1;

[ApiController]
[ServiceFilter(typeof(AuthExceptionFilter<PersonService>))]
[Asp.Versioning.ApiVersion("1")]
[Route("api/auth/v{version:apiVersion}/[controller]")]
[Route("api/auth/[controller]")]
public class PersonController : ControllerBase
{
	private readonly IPersonService _personService;

	public PersonController(IPersonService personService)
	{
		_personService = personService;
	}
	
	#region Create
	
	[HttpPost, Route("[action]")]
	[Authorize("Permission1,Permission2")]
	public async Task<ActionResult> CreatePerson([FromBody] Person personService)
	{
		return Ok(await _personService.CreatePerson(personService));
	}
	
	#endregion
	
	#region Delete
	
	[HttpPost, Route("[action]")]
	[Authorize("Permission3")]
	public async Task<ActionResult> DeletePerson(Guid id)
	{
		return Ok(await _personService.DeletePerson(id));
	}
	
	#endregion

	#region Updates
	
	[HttpPost, Route("[action]")]
	[Authorize("Permission2")]
	public async Task<ActionResult> UpdatePerson([FromBody] Person personService)
	{
		await _personService.UpdatePerson(personService);
		return Ok();
	}
	
	[HttpPatch, Route("[action]/{id}/{firstName}")]
	public async Task<ActionResult> UpdateFirstName(Guid id, string firstName)
	{
		await _personService.UpdateFirstName(id, firstName);
		return Ok();
	}
	
	[HttpPatch, Route("[action]")]
	public async Task<ActionResult> UpdateLastNameAndEmail([FromBody] UpdateLastNameAndEmailReq request)
	{
		await _personService.UpdateLastNameAndEmail(request.Id, request.LastName, request.Email);
		return Ok();
	}

	#endregion

	#region Read Methods - Single
	
	[HttpGet, Route("[action]/{id}")]
	public async Task<ActionResult<Person>> GetById(Guid id)
	{
		return Ok(await _personService.GetById(id));
	}
	
	
	[HttpGet, Route("[action]/{email}")]
	public async Task<ActionResult<Person>> GetByEmail(string email)
	{
		return Ok(await _personService.GetByEmail(email));
	}

	#endregion

	#region Read Methods - List
	
	[HttpGet, Route("[action]/{lastName}")]
	public async Task<ActionResult<List<Person>>> ReadMethod3([FromRoute] string lastName, [FromQuery] int pgSize = 0, [FromQuery] int pgOffset = 0)
	{
		return Ok(await _personService.ReadMethod3(lastName, pgSize, pgOffset));
	}

	#endregion

	#region Query Methods

	[HttpPost, Route("[action]")]
	public async Task<ActionResult<EntityList<Person>>> ReadMethod4([FromBody] ReadMethod4Query readMethod4Query)
	{
		return Ok(await _personService.ReadMethod4(readMethod4Query));
	}

	#endregion
}
