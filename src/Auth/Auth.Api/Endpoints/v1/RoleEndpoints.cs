using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Dyvenix.App1.Auth.Api.Endpoints.v1;

public static class RoleEndpoints
{
	public static IEndpointRouteBuilder MapRoleEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/auth/v1/role")
			.WithTags("Role");

		// Create
		group.MapPost("CreateRole", CreateRole)
			.Produces<Result<RoleDto>>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest);

		// Delete
		group.MapDelete("DeleteRole/{roleId}", DeleteRole)
			.Produces<Result>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		// Update
		group.MapPut("UpdateRole", UpdateRole)
			.Produces<Result>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		// Read
		group.MapGet("GetRoleById/{roleId}", GetRoleById)
			.Produces<Result<RoleDto>>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		group.MapGet("GetRoleByName/{roleName}", GetRoleByName)
			.Produces<Result<RoleDto>>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		group.MapGet("GetAllRoles", GetAllRoles)
			.Produces<Result<List<RoleDto>>>(StatusCodes.Status200OK);

		return app;
	}

	#region Create

	public static async Task<Result<RoleDto>> CreateRole(IRoleService roleService, [FromBody] CreateRoleReq request)
	{
		var role = await roleService.CreateRole(request);
		return Result<RoleDto>.Ok(role);
	}

	#endregion

	#region Delete

	public static async Task<Result> DeleteRole(IRoleService roleService, string roleId)
	{
		await roleService.DeleteRole(roleId);
		return Result.Ok();
	}

	#endregion

	#region Update

	public static async Task<Result> UpdateRole(IRoleService roleService, [FromBody] UpdateRoleReq request)
	{
		await roleService.UpdateRole(request);
		return Result.Ok();
	}

	#endregion

	#region Read

	public static async Task<Result<RoleDto>> GetRoleById(IRoleService roleService, string roleId)
	{
		var role = await roleService.GetRoleById(roleId);
		return Result<RoleDto>.Ok(role);
	}

	public static async Task<Result<RoleDto>> GetRoleByName(IRoleService roleService, string roleName)
	{
		var role = await roleService.GetRoleByName(roleName);
		return Result<RoleDto>.Ok(role);
	}

	public static async Task<Result<List<RoleDto>>> GetAllRoles(IRoleService roleService)
	{
		var roles = await roleService.GetAllRoles();
		return Result<List<RoleDto>>.Ok(roles);
	}

	#endregion
}
