using Dyvenix.App1.Common.Shared.Contracts;
using Dyvenix.App1.Common.Shared.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace Dyvenix.App1.AdAgent.Api.Endpoints;

public static class AdAgentSystemEndpoints
{
    public static IEndpointRouteBuilder MapAdAgentSystemEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/adagent/system")
            .WithTags("System")
            .AllowAnonymous();

        group.MapGet("ping", Ping)
            .Produces<PingResult>(StatusCodes.Status200OK);

        group.MapGet("health", Health)
            .Produces<object>(StatusCodes.Status200OK);

        group.MapGet("getserviceinfo", GetServiceInfo)
            .Produces<object>(StatusCodes.Status200OK);

        return app;
    }

    private static IResult Ping(ClaimsPrincipal user)
    {
        var claims = user.Claims.Select(c => $"{c.Type} = {c.Value}").ToList();
        return Results.Ok(new PingResult(AdAgentConstants.ModuleId, "System"));
    }

    private static async Task<IResult> Health(ISystemService systemService)
    {
        var healthStatus = await systemService.Health();
        return Results.Ok(healthStatus);
    }

    private static async Task<IResult> GetServiceInfo(ISystemService adAgentSystemService)
    {
        var serviceInfo = await adAgentSystemService.GetServiceInfo();
        return Results.Ok(serviceInfo);
    }
}
