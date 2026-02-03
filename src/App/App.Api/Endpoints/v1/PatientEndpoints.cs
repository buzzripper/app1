//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/2/2026 8:28 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Api.Services.v1;
using Dyvenix.App1.App.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.App.Shared.DTOs.v1;
using Dyvenix.App1.App.Shared.Queries.v1;

namespace Dyvenix.App1.App.Api.Controllers.v1;

public static class PatientEndpoints
{
	public static IEndpointRouteBuilder MapPatientEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/app/1/patient")
			.WithTags("Patient");
			
			// Create
			group.MapPost("CreatePatient", CreatePatient)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status409Conflict)
				.RequireAuthorization("Permission1,Permission2");
			
			// Delete
			group.MapPost("DeletePatient", DeletePatient)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status409Conflict)
				.RequireAuthorization("Permission3");
			
			// FullUpdate
			group.MapPost("UpdatePatient", UpdatePatient)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status409Conflict)
				.RequireAuthorization("Permission2");
			
			group.MapPost("UpdateFirstName", UpdateFirstName)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status409Conflict);
			
			group.MapPost("UpdateLastNameAndEmail", UpdateLastNameAndEmail)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status409Conflict);
			
			group.MapGet("GetById", GetById)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status404NotFound);
			
			group.MapGet("GetByEmail", GetByEmail)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status404NotFound);
			
			group.MapGet("GetAllPaging", GetAllPaging)
				.Produces<Guid>(StatusCodes.Status200OK);
			
			group.MapPost("QueryByLastNamePaging", QueryByLastNamePaging)
				.Produces<EntityList<Patient>>(StatusCodes.Status200OK);
	
		return app;
	}
	
	#region Create
	
	public static async Task<IResult> CreatePatient(Patient patient, IPatientService patientService)
	{
		var result = await patientService.CreatePatient(patient);
		return result.ToHttpResult();
	}
	
	#endregion
	
	#region Delete
	
	public static async Task<IResult> DeletePatient(Guid id, IPatientService patientService)
	{
		var result = await patientService.DeletePatient(id);
		return result.ToHttpResult();
	}
	
	#endregion

	#region Updates
	
	public static async Task<IResult> UpdatePatient(Patient patient, IPatientService patientService)
	{
		var result = await patientService.UpdatePatient(patient);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> UpdateFirstName(Guid id, string firstName, IPatientService patientService)
	{
		var result = await patientService.UpdateFirstName(id, firstName);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> UpdateLastNameAndEmail([FromBody] UpdateLastNameAndEmailReq request, IPatientService patientService)
	{
		var result = await patientService.UpdateLastNameAndEmail(request.Id, request.LastName, request.Email);
		return result.ToHttpResult();
	}

	#endregion

	#region Read Methods - Single
	
	public static async Task<IResult> GetById(Guid id, IPatientService patientService)
	{
		var result = await patientService.GetById(id);
		return result.ToHttpResult();
	}
	
	
	public static async Task<IResult> GetByEmail(string email, IPatientService patientService)
	{
		var result = await patientService.GetByEmail(email);
		return result.ToHttpResult();
	}

	#endregion

	#region Read Methods - List
	
	public static async Task<IResult> GetAllPaging(IPatientService patientService, [FromQuery] int pgSize = 0, [FromQuery] int pgOffset = 0)
	{
		var result = await patientService.GetAllPaging(pgSize, pgOffset);
		return result.ToHttpResult();
	}

	#endregion

	#region Query Methods

	public static async Task<IResult> QueryByLastNamePaging(IPatientService patientService, [FromBody] QueryByLastNamePagingQuery queryByLastNamePagingQuery)
	{
		var result = await patientService.QueryByLastNamePaging(queryByLastNamePagingQuery);
		return result.ToHttpResult();
	}

	#endregion
}
