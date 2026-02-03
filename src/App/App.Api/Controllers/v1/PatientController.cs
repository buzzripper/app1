//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/1/2026 9:58 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Api.Services.v1;
using Dyvenix.App1.App.Shared.DTOs.v1;
using Dyvenix.App1.App.Shared.Queries.v1;

namespace Dyvenix.App1.App.Api.Controllers.v1;

[ApiController]
[ServiceFilter(typeof(ApiExceptionFilter<PatientService>))]
[Asp.Versioning.ApiVersion("1")]
[Route("api/app/v{version:apiVersion}/[controller]")]
[Route("api/app/[controller]")]
public class PatientController : ControllerBase
{
	private readonly IPatientService _patientService;

	public PatientController(IPatientService patientService)
	{
		_patientService = patientService;
	}
	
	#region Create
	
	[HttpPost, Route("[action]")]
	[Authorize("Permission1,Permission2")]
	public async Task<ActionResult> CreatePatient([FromBody] Patient patientService)
	{
		return Ok(await _patientService.CreatePatient(patientService));
	}
	
	#endregion
	
	#region Delete
	
	[HttpPost, Route("[action]")]
	[Authorize("Permission3")]
	public async Task<ActionResult> DeletePatient(Guid id)
	{
		return Ok(await _patientService.DeletePatient(id));
	}
	
	#endregion

	#region Updates
	
	[HttpPost, Route("[action]")]
	[Authorize("Permission2")]
	public async Task<ActionResult> UpdatePatient([FromBody] Patient patientService)
	{
		await _patientService.UpdatePatient(patientService);
		return Ok();
	}
	
	[HttpPatch, Route("[action]/{id}/{firstName}")]
	public async Task<ActionResult> UpdateFirstName(Guid id, string firstName)
	{
		await _patientService.UpdateFirstName(id, firstName);
		return Ok();
	}
	
	[HttpPatch, Route("[action]")]
	public async Task<ActionResult> UpdateLastNameAndEmail([FromBody] UpdateLastNameAndEmailReq request)
	{
		await _patientService.UpdateLastNameAndEmail(request.Id, request.LastName, request.Email);
		return Ok();
	}

	#endregion

	#region Read Methods - Single
	
	[HttpGet, Route("[action]/{id}")]
	public async Task<ActionResult<Patient>> GetById(Guid id)
	{
		return Ok(await _patientService.GetById(id));
	}
	
	
	[HttpGet, Route("[action]/{email}")]
	public async Task<ActionResult<Patient>> GetByEmail(string email)
	{
		return Ok(await _patientService.GetByEmail(email));
	}

	#endregion

	#region Read Methods - List
	
	[HttpGet, Route("[action]")]
	public async Task<ActionResult<List<Patient>>> GetAllPaging([FromQuery] int pgSize = 0, [FromQuery] int pgOffset = 0)
	{
		return Ok(await _patientService.GetAllPaging(pgSize, pgOffset));
	}

	#endregion

	#region Query Methods

	[HttpPost, Route("[action]")]
	public async Task<ActionResult<EntityList<Patient>>> QueryByLastNamePaging([FromBody] QueryByLastNamePagingQuery queryByLastNamePagingQuery)
	{
		return Ok(await _patientService.QueryByLastNamePaging(queryByLastNamePagingQuery));
	}

	#endregion
}
