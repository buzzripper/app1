using Dyvenix.App1.Shared.Interfaces;
using Dyvenix.System.Apis.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace App1.Api.Endpoints;

public class SystemEndpointModule : IEndpointModule
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/system").WithTags("System");

		group.MapGet("/alive", Alive).WithName("SystemAlive").AllowAnonymous();
		group.MapGet("/health", Health).WithName("SystemHealth").AllowAnonymous();
	}

	[AllowAnonymous]
	private static async Task<IResult> Alive(IApp1SystemService systemService)
	{
		var health = await systemService.Alive();
		return Results.Ok(health);
	}

	[AllowAnonymous]
	private static async Task<IResult> Health(IApp1SystemService systemService)
	{
		var health = await systemService.Health();
		return Results.Ok(health);
	}
}
