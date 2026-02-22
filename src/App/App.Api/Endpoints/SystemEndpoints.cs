using Dyvenix.App1.App.Shared.Contracts;
using Dyvenix.App1.Common.Shared.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace Dyvenix.App1.App.Api.Endpoints;

public static class SystemEndpoints
{
    public static IEndpointRouteBuilder MapAppSystemEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/app/system")
            .WithTags("System")
            .AllowAnonymous();

        group.MapGet("ping", Ping)
            .Produces<PingResult>(StatusCodes.Status200OK);

        group.MapGet("health", Health)
            .Produces<object>(StatusCodes.Status200OK);

        return app;
    }

    private static IResult Ping(ClaimsPrincipal user)
    {
        var claims = user.Claims.Select(c => $"{c.Type} = {c.Value}").ToList();
        return Results.Ok(new PingResult(AppConstants.ModuleId, "System"));
    }

    private static async Task<IResult> Health(IAppSystemService systemService)
    {
        var healthStatus = await systemService.Health();
        return Results.Ok(healthStatus);
    }
}
