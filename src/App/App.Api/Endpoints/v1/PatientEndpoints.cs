//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/3/2026 5:33 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Api.Services.v1;
using Dyvenix.App1.App.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.App.Shared.DTOs.v1;
using Dyvenix.App1.App.Shared.Requests.v1;

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
			
			group.MapGet("GetByIdWithInvoices", GetByIdWithInvoices)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status404NotFound);
			
			group.MapGet("ReqByEmail", ReqByEmail)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status404NotFound);
			
			group.MapGet("GetActive", GetActive)
				.Produces<Guid>(StatusCodes.Status200OK);
			
			group.MapPost("GetAllPaging", GetAllPaging)
				.Produces<EntityList<Patient>>(StatusCodes.Status200OK);
			
			group.MapPost("QueryByLastNamePaging", QueryByLastNamePaging)
				.Produces<EntityList<Patient>>(StatusCodes.Status200OK);
			
			group.MapPost("QueryByLastNameSorting", QueryByLastNameSorting)
				.Produces<EntityList<Patient>>(StatusCodes.Status200OK);
			
			group.MapPost("QueryByLastNamePagingSorting", QueryByLastNamePagingSorting)
				.Produces<EntityList<Patient>>(StatusCodes.Status200OK);
			
			group.MapPost("GetAllSorting", GetAllSorting)
				.Produces<EntityList<Patient>>(StatusCodes.Status200OK);
			
			group.MapPost("QueryByLastEmailOpt", QueryByLastEmailOpt)
				.Produces<EntityList<Patient>>(StatusCodes.Status200OK);
			
			group.MapPost("GetAllPagingSorting", GetAllPagingSorting)
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
	
	public static async Task<IResult> DeletePatient(IPatientService patientService, Guid id)
	{
		var result = await patientService.DeletePatient(id);
		return result.ToHttpResult();
	}
	
	#endregion

	#region Updates
	
	public static async Task<IResult> UpdatePatient(IPatientService patientService, Patient patient)
	{
		var result = await patientService.UpdatePatient(patient);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> UpdateFirstName(IPatientService patientService, Guid id, string firstName)
	{
		var result = await patientService.UpdateFirstName(id, firstName);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> UpdateLastNameAndEmail(IPatientService patientService, [FromBody] UpdateLastNameAndEmailReq request)
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
	
	public static async Task<IResult> GetByIdWithInvoices(Guid id, IPatientService patientService)
	{
		var result = await patientService.GetByIdWithInvoices(id);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> ReqByEmail(string email, IPatientService patientService)
	{
		var result = await patientService.ReqByEmail(email);
		return result.ToHttpResult();
	}

	#endregion

	#region Read Methods - List
	
	public static async Task<IResult> GetActive(IPatientService patientService)
	{
		var result = await patientService.GetActive();
		return result.ToHttpResult();
	}

	#endregion

	#region Request Methods


	public static async Task<IResult> GetAllPaging(IPatientService patientService, [FromBody] GetAllPagingRequest getAllPagingRequest)
	{
		var result = await patientService.GetAllPaging(getAllPagingRequest);
		return result.ToHttpResult();
	}

	public static async Task<IResult> QueryByLastNamePaging(IPatientService patientService, [FromBody] QueryByLastNamePagingRequest queryByLastNamePagingRequest)
	{
		var result = await patientService.QueryByLastNamePaging(queryByLastNamePagingRequest);
		return result.ToHttpResult();
	}

	public static async Task<IResult> QueryByLastNameSorting(IPatientService patientService, [FromBody] QueryByLastNameSortingRequest queryByLastNameSortingRequest)
	{
		var result = await patientService.QueryByLastNameSorting(queryByLastNameSortingRequest);
		return result.ToHttpResult();
	}

	public static async Task<IResult> QueryByLastNamePagingSorting(IPatientService patientService, [FromBody] QueryByLastNamePagingSortingRequest queryByLastNamePagingSortingRequest)
	{
		var result = await patientService.QueryByLastNamePagingSorting(queryByLastNamePagingSortingRequest);
		return result.ToHttpResult();
	}

	public static async Task<IResult> GetAllSorting(IPatientService patientService, [FromBody] GetAllSortingRequest getAllSortingRequest)
	{
		var result = await patientService.GetAllSorting(getAllSortingRequest);
		return result.ToHttpResult();
	}

	public static async Task<IResult> QueryByLastEmailOpt(IPatientService patientService, [FromBody] QueryByLastEmailOptRequest queryByLastEmailOptRequest)
	{
		var result = await patientService.QueryByLastEmailOpt(queryByLastEmailOptRequest);
		return result.ToHttpResult();
	}

	public static async Task<IResult> GetAllPagingSorting(IPatientService patientService, [FromBody] GetAllPagingSortingRequest getAllPagingSortingRequest)
	{
		var result = await patientService.GetAllPagingSorting(getAllPagingSortingRequest);
		return result.ToHttpResult();
	}

	#endregion
}
