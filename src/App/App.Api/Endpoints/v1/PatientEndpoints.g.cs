//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/13/2026 8:31 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Api.Services.v1;
using Dyvenix.App1.Common.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;

namespace Dyvenix.App1.App.Endpoints.v1;

public static class PatientEndpoints
{
	public static IEndpointRouteBuilder MapPatientEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/app/v1/patient")
			.WithTags("Patient");
		
		// Create
		
		group.MapPut("CreatePatient", CreatePatient)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict)
			.RequireAuthorization("Permission1,Permission2");
		
		// Delete
		
		group.MapDelete("DeletePatient", DeletePatient)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict)
			.RequireAuthorization("Permission3");
		
		// Full Update
		
		group.MapPatch("UpdatePatient", UpdatePatient)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict)
			.RequireAuthorization("Permission2");
		
		// Update
		
		group.MapPatch("UpdateFirstName", UpdateFirstName)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPatch("UpdateLastNameAndEmail", UpdateLastNameAndEmail)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Read - Single
		
		group.MapGet("GetById/{id}", GetById)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapGet("GetByEmail/{email}", GetByEmail)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapGet("GetByIdWithInvoices/{id}", GetByIdWithInvoices)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict);
		
		// Read - List
		
		group.MapPost("GetAllPaging", GetAllPaging)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPost("SearchByLastNamePaging", SearchByLastNamePaging)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPost("SearchByLastNameSorting", SearchByLastNameSorting)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPost("SearchByLastNamePagingSorting", SearchByLastNamePagingSorting)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPost("GetAllSorting", GetAllSorting)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPost("SearchByLastEmailOpt", SearchByLastEmailOpt)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPost("SearchByEmail", SearchByEmail)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapGet("GetActive", GetActive)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPost("GetAllPagingSorting", GetAllPagingSorting)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapGet("SearchActiveLastName/{lastName}", SearchActiveLastName)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
	
		return app;
	}
	
	#region Create
	
	public static async Task<IResult> CreatePatient(IPatientService patientService, Patient patient)
	{
		await patientService.CreatePatient(patient);
		return Results.Ok();
	}
	
	#endregion
	
	#region Delete
	
	public static async Task<Result> DeletePatient(IPatientService patientService, [FromBody] DeleteReq deleteReq)
	{
		await patientService.DeletePatient(deleteReq.Id);
		return Result.Ok();
	}
	
	#endregion

	#region Updates
	
	public static async Task<Result<byte[]>> UpdatePatient(IPatientService patientService, Patient patient)
	{
		var rowVersion = await patientService.UpdatePatient(patient);
		return Result<byte[]>.Ok(rowVersion);
	}
	
	
	public static async Task<Result<byte[]>> UpdateFirstName(IPatientService patientService, [FromBody] UpdateFirstNameReq request)
	{
		var rowVersion = await patientService.UpdateFirstName(request);
		return Result<byte[]>.Ok(rowVersion);
	}
	
	
	public static async Task<Result<byte[]>> UpdateLastNameAndEmail(IPatientService patientService, [FromBody] UpdateLastNameAndEmailReq request)
	{
		var rowVersion = await patientService.UpdateLastNameAndEmail(request);
		return Result<byte[]>.Ok(rowVersion);
	}

	#endregion

	#region Read Methods - Single
	
	public static async Task<Result<Patient>> GetById(IPatientService patientService, Guid id)
	{
		var patient = await patientService.GetById(id);
		return Result<Patient>.Ok(patient);
	}
	
	public static async Task<Result<Patient>> GetByEmail(IPatientService patientService, string email)
	{
		var patient = await patientService.GetByEmail(email);
		return Result<Patient>.Ok(patient);
	}
	
	public static async Task<Result<Patient>> GetByIdWithInvoices(IPatientService patientService, Guid id)
	{
		var patient = await patientService.GetByIdWithInvoices(id);
		return Result<Patient>.Ok(patient);
	}

	#endregion

	#region Read Methods - List
	
	public static async Task<Result<ListPage<Patient>>> GetAllPaging(IPatientService patientService, GetAllPagingReq getAllPagingReq)
	{
		var data = await patientService.GetAllPaging(getAllPagingReq);
		return Result<ListPage<Patient>>.Ok(data);
	}
	
	public static async Task<Result<ListPage<Patient>>> SearchByLastNamePaging(IPatientService patientService, SearchByLastNamePagingReq searchByLastNamePagingReq)
	{
		var data = await patientService.SearchByLastNamePaging(searchByLastNamePagingReq);
		return Result<ListPage<Patient>>.Ok(data);
	}
	
	public static async Task<Result<List<Patient>>> SearchByLastNameSorting(IPatientService patientService, SearchByLastNameSortingReq searchByLastNameSortingReq)
	{
		var data = await patientService.SearchByLastNameSorting(searchByLastNameSortingReq);
		return Result<List<Patient>>.Ok(data);
	}
	
	public static async Task<Result<ListPage<Patient>>> SearchByLastNamePagingSorting(IPatientService patientService, SearchByLastNamePagingSortingReq searchByLastNamePagingSortingReq)
	{
		var data = await patientService.SearchByLastNamePagingSorting(searchByLastNamePagingSortingReq);
		return Result<ListPage<Patient>>.Ok(data);
	}
	
	public static async Task<Result<List<Patient>>> GetAllSorting(IPatientService patientService, GetAllSortingReq getAllSortingReq)
	{
		var data = await patientService.GetAllSorting(getAllSortingReq);
		return Result<List<Patient>>.Ok(data);
	}
	
	public static async Task<Result<List<Patient>>> SearchByLastEmailOpt(IPatientService patientService, SearchByLastEmailOptReq searchByLastEmailOptReq)
	{
		var data = await patientService.SearchByLastEmailOpt(searchByLastEmailOptReq);
		return Result<List<Patient>>.Ok(data);
	}
	
	public static async Task<Result<List<Patient>>> SearchByEmail(IPatientService patientService, SearchByEmailReq searchByEmailReq)
	{
		var data = await patientService.SearchByEmail(searchByEmailReq);
		return Result<List<Patient>>.Ok(data);
	}
	
	public static async Task<Result<List<Patient>>> GetActive(IPatientService patientService)
	{
		var data = await patientService.GetActive();
		return Result<List<Patient>>.Ok(data);
	}
	
	public static async Task<Result<ListPage<Patient>>> GetAllPagingSorting(IPatientService patientService, GetAllPagingSortingReq getAllPagingSortingReq)
	{
		var data = await patientService.GetAllPagingSorting(getAllPagingSortingReq);
		return Result<ListPage<Patient>>.Ok(data);
	}
	
	public static async Task<Result<List<Patient>>> SearchActiveLastName(IPatientService patientService, [FromRoute] string lastName)
	{
		var data = await patientService.SearchActiveLastName(lastName);
		return Result<List<Patient>>.Ok(data);
	}

	#endregion
}
