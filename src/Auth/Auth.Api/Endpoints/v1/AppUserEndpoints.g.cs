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
using Dyvenix.App1.Auth.Api.Services.v1;
using Dyvenix.App1.Common.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Endpoints.v1;

public static class AppUserEndpoints
{
	public static IEndpointRouteBuilder MapAppUserEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/auth/v1/appuser")
			.WithTags("AppUser");
		
		// Create
		
		group.MapPut("CreateAppUser", CreateAppUser)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Delete
		
		group.MapDelete("DeleteAppUser", DeleteAppUser)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Full Update
		
		group.MapPatch("UpdateAppUser", UpdateAppUser)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Update
		
		group.MapPatch("UpdateUsername", UpdateUsername)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Read - Single
		
		group.MapGet("GetById/{id}", GetById)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);
		
		// Read - List
		
		group.MapPost("ReqByUsername", ReqByUsername)
			.Produces<Guid>(StatusCodes.Status200OK);
	
		return app;
	}
	
	#region Create
	
	public static async Task<IResult> CreateAppUser(IAppUserService appUserService, AppUser appUser)
	{
		await appUserService.CreateAppUser(appUser);
		return Results.Ok();
	}
	
	#endregion
	
	#region Delete
	
	public static async Task<Result> DeleteAppUser(IAppUserService appUserService, [FromBody] DeleteReq deleteReq)
	{
		await appUserService.DeleteAppUser(deleteReq.Id);
		return Result.Ok();
	}
	
	#endregion

	#region Updates
	
	public static async Task<Result> UpdateAppUser(IAppUserService appUserService, AppUser appUser)
	{
		await appUserService.UpdateAppUser(appUser);
		return Result.Ok();
	}
	
	
	public static async Task<Result> UpdateUsername(IAppUserService appUserService, [FromBody] UpdateUsernameReq request)
	{
		await appUserService.UpdateUsername(request);
		return Result.Ok();
	}

	#endregion

	#region Read Methods - Single
	
	public static async Task<Result<AppUser>> GetById(IAppUserService appUserService, Guid id)
	{
		var appUser = await appUserService.GetById(id);
		return Result<AppUser>.Ok(appUser);
	}

	#endregion

	#region Read Methods - List
	
	public static async Task<Result<List<AppUser>>> ReqByUsername(IAppUserService appUserService, ReqByUsernameReq reqByUsernameReq)
	{
		var data = await appUserService.ReqByUsername(reqByUsernameReq);
		return Result<List<AppUser>>.Ok(data);
	}

	#endregion
}
