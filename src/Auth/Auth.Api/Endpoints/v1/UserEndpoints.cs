using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Dyvenix.App1.Auth.Api.Endpoints.v1;

public static class UserEndpoints
{
	public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/auth/v1/user")
			.WithTags("User");

		// Create
		group.MapPost("CreateUser", CreateUser)
			.Produces<Result<UserDto>>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest);

		// Delete
		group.MapDelete("DeleteUser/{userId}", DeleteUser)
			.Produces<Result>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		// Update
		group.MapPut("UpdateUser", UpdateUser)
			.Produces<Result>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		// Roles
		group.MapGet("GetUserRoles/{userId}", GetUserRoles)
			.Produces<Result<List<string>>>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		group.MapPost("AddToRole", AddToRole)
			.Produces<Result>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest);

		group.MapPost("RemoveFromRole", RemoveFromRole)
			.Produces<Result>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest);

		// Claims
		group.MapGet("GetUserClaims/{userId}", GetUserClaims)
			.Produces<Result<List<UserClaimDto>>>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		group.MapPost("AddUserClaim", AddUserClaim)
			.Produces<Result>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest);

		group.MapPost("RemoveUserClaim", RemoveUserClaim)
			.Produces<Result>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest);

		// Read
		group.MapGet("GetUserById/{userId}", GetUserById)
			.Produces<Result<UserDto>>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		group.MapGet("GetUserByEmail/{email}", GetUserByEmail)
			.Produces<Result<UserDto>>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		group.MapGet("GetAllUsers", GetAllUsers)
			.Produces<Result<List<UserDto>>>(StatusCodes.Status200OK);

		return app;
	}

	#region Create

	public static async Task<Result<UserDto>> CreateUser(IUserService userService, [FromBody] CreateUserReq request)
	{
		var user = await userService.CreateUser(request);
		return Result<UserDto>.Ok(user);
	}

	#endregion

	#region Delete

	public static async Task<Result> DeleteUser(IUserService userService, string userId)
	{
		await userService.DeleteUser(userId);
		return Result.Ok();
	}

	#endregion

	#region Update

	public static async Task<Result> UpdateUser(IUserService userService, [FromBody] UpdateUserReq request)
	{
		await userService.UpdateUser(request);
		return Result.Ok();
	}

	#endregion

	#region Roles

	public static async Task<Result<List<string>>> GetUserRoles(IUserService userService, string userId)
	{
		var roles = await userService.GetUserRoles(userId);
		return Result<List<string>>.Ok(roles);
	}

	public static async Task<Result> AddToRole(IUserService userService, [FromBody] AddToRoleReq request)
	{
		await userService.AddToRole(request);
		return Result.Ok();
	}

	public static async Task<Result> RemoveFromRole(IUserService userService, [FromBody] RemoveFromRoleReq request)
	{
		await userService.RemoveFromRole(request);
		return Result.Ok();
	}

	#endregion

	#region Claims

	public static async Task<Result<List<UserClaimDto>>> GetUserClaims(IUserService userService, string userId)
	{
		var claims = await userService.GetUserClaims(userId);
		return Result<List<UserClaimDto>>.Ok(claims);
	}

	public static async Task<Result> AddUserClaim(IUserService userService, [FromBody] AddUserClaimReq request)
	{
		await userService.AddUserClaim(request);
		return Result.Ok();
	}

	public static async Task<Result> RemoveUserClaim(IUserService userService, [FromBody] RemoveUserClaimReq request)
	{
		await userService.RemoveUserClaim(request);
		return Result.Ok();
	}

	#endregion

	#region Read

	public static async Task<Result<UserDto>> GetUserById(IUserService userService, string userId)
	{
		var user = await userService.GetUserById(userId);
		return Result<UserDto>.Ok(user);
	}

	public static async Task<Result<UserDto>> GetUserByEmail(IUserService userService, string email)
	{
		var user = await userService.GetUserByEmail(email);
		return Result<UserDto>.Ok(user);
	}

	public static async Task<Result<List<UserDto>>> GetAllUsers(IUserService userService)
	{
		var users = await userService.GetAllUsers();
		return Result<List<UserDto>>.Ok(users);
	}

	#endregion
}
