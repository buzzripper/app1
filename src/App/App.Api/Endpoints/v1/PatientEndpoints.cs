//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/7/2026 3:16 PM. Any changes made to it will be lost.
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
		var group = app.MapGroup("api/app/v1/patient")
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
		
		// Full Update
		
		group.MapPost("UpdatePatient", UpdatePatient)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict)
			.RequireAuthorization("Permission2");
		
		// Update
		
		group.MapPost("UpdateFirstName", UpdateFirstName)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPost("UpdateLastNameAndEmail", UpdateLastNameAndEmail)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Read - Single
		
		group.MapGet("GetById/{id}", GetById)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);
		
		group.MapGet("GetByEmail/{email}", GetByEmail)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);
		
		group.MapGet("GetByIdWithInvoices/{id}", GetByIdWithInvoices)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);
		
		// Read - List
		
		group.MapPost("GetAllPaging", GetAllPaging)
			.Produces<Guid>(StatusCodes.Status200OK);
		
		group.MapPost("SearchByLastNamePaging", SearchByLastNamePaging)
			.Produces<Guid>(StatusCodes.Status200OK);
		
		group.MapPost("SearchByLastNameSorting", SearchByLastNameSorting)
			.Produces<Guid>(StatusCodes.Status200OK);
		
		group.MapPost("SearchByLastNamePagingSorting", SearchByLastNamePagingSorting)
			.Produces<Guid>(StatusCodes.Status200OK);
		
		group.MapPost("GetAllSorting", GetAllSorting)
			.Produces<Guid>(StatusCodes.Status200OK);
		
		group.MapPost("SearchByLastEmailOpt", SearchByLastEmailOpt)
			.Produces<Guid>(StatusCodes.Status200OK);
		
		group.MapPost("SearchByEmail", SearchByEmail)
			.Produces<Guid>(StatusCodes.Status200OK);
		
		group.MapGet("GetActive", GetActive)
			.Produces<Guid>(StatusCodes.Status200OK);
		
		group.MapPost("GetAllPagingSorting", GetAllPagingSorting)
			.Produces<Guid>(StatusCodes.Status200OK);
		
		group.MapGet("SearchActiveLastName/{lastName}", SearchActiveLastName)
			.Produces<Guid>(StatusCodes.Status200OK);
	
		return app;
	}
	
	#region Create
	
	public static async Task<IResult> CreatePatient(IPatientService patientService, Patient patient)
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
	
	public static async Task<IResult> GetById(IPatientService patientService, Guid id)
	{
		var result = await patientService.GetById(id);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> GetByEmail(IPatientService patientService, string email)
	{
		var result = await patientService.GetByEmail(email);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> GetByIdWithInvoices(IPatientService patientService, Guid id)
	{
		var result = await patientService.GetByIdWithInvoices(id);
		return result.ToHttpResult();
	}

	#endregion

	#region Read Methods - List
	
	public static async Task<IResult> GetAllPaging(IPatientService patientService, GetAllPagingReq getAllPagingReq)
	{
		var result = await patientService.GetAllPaging(getAllPagingReq);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> SearchByLastNamePaging(IPatientService patientService, SearchByLastNamePagingReq searchByLastNamePagingReq)
	{
		var result = await patientService.SearchByLastNamePaging(searchByLastNamePagingReq);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> SearchByLastNameSorting(IPatientService patientService, SearchByLastNameSortingReq searchByLastNameSortingReq)
	{
		var result = await patientService.SearchByLastNameSorting(searchByLastNameSortingReq);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> SearchByLastNamePagingSorting(IPatientService patientService, SearchByLastNamePagingSortingReq searchByLastNamePagingSortingReq)
	{
		var result = await patientService.SearchByLastNamePagingSorting(searchByLastNamePagingSortingReq);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> GetAllSorting(IPatientService patientService, GetAllSortingReq getAllSortingReq)
	{
		var result = await patientService.GetAllSorting(getAllSortingReq);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> SearchByLastEmailOpt(IPatientService patientService, SearchByLastEmailOptReq searchByLastEmailOptReq)
	{
		var result = await patientService.SearchByLastEmailOpt(searchByLastEmailOptReq);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> SearchByEmail(IPatientService patientService, SearchByEmailReq searchByEmailReq)
	{
		var result = await patientService.SearchByEmail(searchByEmailReq);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> GetActive(IPatientService patientService)
	{
		var result = await patientService.GetActive();
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> GetAllPagingSorting(IPatientService patientService, GetAllPagingSortingReq getAllPagingSortingReq)
	{
		var result = await patientService.GetAllPagingSorting(getAllPagingSortingReq);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> SearchActiveLastName(IPatientService patientService, [FromRoute] string lastName)
	{
		var result = await patientService.SearchActiveLastName(lastName);
		return result.ToHttpResult();
	}

	#endregion
}
