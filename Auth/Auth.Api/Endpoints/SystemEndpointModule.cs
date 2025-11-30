using Dyvenix.Auth.Shared.Interfaces;
using Dyvenix.System.Apis.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Auth.Api.Endpoints;

public class SystemEndpointModule : IEndpointModule
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/system").WithTags("System");

		group.MapGet("/alive", Alive).WithName("SystemAlive").AllowAnonymous();
		group.MapGet("/health", Health).WithName("SystemHealth").AllowAnonymous();
	}

	[AllowAnonymous]
	private static async Task<IResult> Alive(IAuthSystemService systemService)
	{
		var health = await systemService.Alive();
		return Results.Ok(health);
	}

	[AllowAnonymous]
	private static async Task<IResult> Health(IAuthSystemService systemService)
	{
		var health = await systemService.Health();
		return Results.Ok(health);
	}
}
