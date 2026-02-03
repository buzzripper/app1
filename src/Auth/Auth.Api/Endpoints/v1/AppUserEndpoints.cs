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
using Dyvenix.App1.Auth.Api.Services.v1;
using Dyvenix.App1.Auth.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Api.Controllers.v1;

public static class AppUserEndpoints
{
	public static IEndpointRouteBuilder MapAppUserEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/auth/1/appuser")
			.WithTags("AppUser");
			
			// Create
			group.MapPost("CreateAppUser", CreateAppUser)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status409Conflict);
			
			// Delete
			group.MapPost("DeleteAppUser", DeleteAppUser)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status409Conflict);
			
			// FullUpdate
			group.MapPost("UpdateAppUser", UpdateAppUser)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status409Conflict);
			
			group.MapPost("UpdateUsername", UpdateUsername)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status409Conflict);
			
			group.MapGet("GetById", GetById)
				.Produces<Guid>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status404NotFound);
			
			group.MapPost("ReqByUsername", ReqByUsername)
				.Produces<EntityList<AppUser>>(StatusCodes.Status200OK);
	
		return app;
	}
	
	#region Create
	
	public static async Task<IResult> CreateAppUser(AppUser appUser, IAppUserService appUserService)
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
	
	public static async Task<IResult> GetById(Guid id, IAppUserService appUserService)
	{
		var result = await appUserService.GetById(id);
		return result.ToHttpResult();
	}

	#endregion

	#region Request Methods


	public static async Task<IResult> ReqByUsername(IAppUserService appUserService, [FromBody] ReqByUsernameRequest reqByUsernameRequest)
	{
		var result = await appUserService.ReqByUsername(reqByUsernameRequest);
		return result.ToHttpResult();
	}

	#endregion
}
