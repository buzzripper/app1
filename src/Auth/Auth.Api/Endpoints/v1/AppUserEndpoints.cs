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
using Dyvenix.App1.Auth.Api.Services.v1;
using Dyvenix.App1.Auth.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Api.Controllers.v1;

public static class AppUserEndpoints
{
	public static IEndpointRouteBuilder MapAppUserEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/auth/v1/appuser")
			.WithTags("AppUser");
		
		// Create
		
		group.MapPost("CreateAppUser", CreateAppUser)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Delete
		
		group.MapPost("DeleteAppUser", DeleteAppUser)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Full Update
		
		group.MapPost("UpdateAppUser", UpdateAppUser)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Update
		
		group.MapPost("UpdateUsername", UpdateUsername)
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
		var result = await appUserService.CreateAppUser(appUser);
		return result.ToHttpResult();
	}
	
	#endregion
	
	#region Delete
	
	public static async Task<IResult> DeleteAppUser(IAppUserService appUserService, Guid id)
	{
		var result = await appUserService.DeleteAppUser(id);
		return result.ToHttpResult();
	}
	
	#endregion

	#region Updates
	
	public static async Task<IResult> UpdateAppUser(IAppUserService appUserService, AppUser appUser)
	{
		var result = await appUserService.UpdateAppUser(appUser);
		return result.ToHttpResult();
	}
	
	public static async Task<IResult> UpdateUsername(IAppUserService appUserService, Guid id, string username)
	{
		var result = await appUserService.UpdateUsername(id, username);
		return result.ToHttpResult();
	}

	#endregion

	#region Read Methods - Single
	
	public static async Task<IResult> GetById(IAppUserService appUserService, Guid id)
	{
		var result = await appUserService.GetById(id);
		return result.ToHttpResult();
	}

	#endregion

	#region Read Methods - List
	
	public static async Task<IResult> ReqByUsername(IAppUserService appUserService, ReqByUsernameReq reqByUsernameReq)
	{
		var result = await appUserService.ReqByUsername(reqByUsernameReq);
		return result.ToHttpResult();
	}

	#endregion
}
