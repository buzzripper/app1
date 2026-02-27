//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/27/2026 4:53 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Dyvenix.App1.Auth.Data.Entities;
using Dyvenix.App1.Auth.Api.Services.v1;
using Dyvenix.App1.Common.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Common.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Endpoints.v1;

public static class TenantEndpoints
{
	public static IEndpointRouteBuilder MapTenantEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/auth/v1/tenant")
			.WithTags("Tenant");
		
		// Delete
		
		group.MapDelete("DeleteTenant", DeleteTenant)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Update
		
		group.MapPatch("UpdateName", UpdateName)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Read - Single
		
		group.MapGet("GetById/{id}", GetById)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);
		
		// Read - List
		
		group.MapGet("GetAll", GetAll)
			.Produces<Guid>(StatusCodes.Status200OK);
	
		return app;
	}
	
	#region Delete
	
	public static async Task<Result> DeleteTenant(ITenantService tenantService, [FromBody] DeleteReq deleteReq)
	{
		await tenantService.DeleteTenant(deleteReq.Id);
		return Result.Ok();
	}
	
	#endregion

	#region Updates
	
	public static async Task<Result> UpdateName(ITenantService tenantService, [FromBody] UpdateNameReq request)
	{
		await tenantService.UpdateName(request);
		return Result.Ok();
	}

	#endregion

	#region Read Methods - Single
	
	public static async Task<Result<Dto2>> GetById(ITenantService tenantService, Guid id)
	{
		var dto2 = await tenantService.GetById(id);
		return Result<Dto2>.Ok(dto2);
	}

	#endregion

	#region Read Methods - List
	
	public static async Task<Result<IReadOnlyList<Dto1>>> GetAll(ITenantService tenantService)
	{
		var data = await tenantService.GetAll();
		return Result<IReadOnlyList<Dto1>>.Ok(data);
	}

	#endregion
}
