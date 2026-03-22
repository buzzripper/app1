using Dyvenix.App1.App.Api.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.DTOs;
using Dyvenix.App1.Common.Shared.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Dyvenix.App1.App.Endpoints.v1;

public static class ClientAuthEndpoints
{
	public static IEndpointRouteBuilder MapClientAuthEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/app/v1/clientauth")
			.WithTags("ClientAuth");

		group.MapGet("GetTenantById/{id}", GetTenantById)
			.Produces<TenantDto>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		group.MapGet("GetTenantBySlug/{slug}", GetTenantBySlug)
			.Produces<TenantDto>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		group.MapGet("GetAllTenants", GetAllTenants)
			.Produces<IReadOnlyList<TenantDto>>(StatusCodes.Status200OK);

		group.MapPost("CreateTenant", CreateTenant)
			.Produces<Guid>(StatusCodes.Status200OK);

		group.MapPut("UpdateTenant", UpdateTenant)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		group.MapDelete("DeleteTenant", DeleteTenant)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		return app;
	}

	public static async Task<Result<TenantDto?>> GetTenantById(IClientAuthService clientAuthService, Guid id)
	{
		var dto = await clientAuthService.GetTenantById(id);
		return Result<TenantDto?>.Ok(dto);
	}

	public static async Task<Result<TenantDto?>> GetTenantBySlug(IClientAuthService clientAuthService, string slug)
	{
		var dto = await clientAuthService.GetTenantBySlug(Uri.UnescapeDataString(slug));
		return Result<TenantDto?>.Ok(dto);
	}

	public static async Task<Result<IReadOnlyList<TenantDto>>> GetAllTenants(IClientAuthService clientAuthService)
	{
		var data = await clientAuthService.GetAllTenants();
		return Result<IReadOnlyList<TenantDto>>.Ok(data);
	}

	public static async Task<Result<Guid>> CreateTenant(IClientAuthService clientAuthService, [FromBody] CreateTenantReq request)
	{
		var id = await clientAuthService.CreateTenant(request);
		return Result<Guid>.Ok(id);
	}

	public static async Task<Result> UpdateTenant(IClientAuthService clientAuthService, [FromBody] UpdateTenantReq request)
	{
		await clientAuthService.UpdateTenant(request);
		return Result.Ok();
	}

	public static async Task<Result> DeleteTenant(IClientAuthService clientAuthService, [FromBody] DeleteReq deleteReq)
	{
		await clientAuthService.DeleteTenant(deleteReq.Id);
		return Result.Ok();
	}
}
