using App1.Auth.Api;
using Dyvenix.App1.Common.Shared.Contracts;
using Dyvenix.App1.Common.Shared.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Dyvenix.App1.Auth.Api.Endpoints;

public static class AuthSystemEndpoints
{
    public static IEndpointRouteBuilder MapAuthSystemEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth/system")
            .WithTags("System")
            .AllowAnonymous();

        group.MapGet("ping", Ping)
            .Produces<PingResult>(StatusCodes.Status200OK);

        group.MapGet("health", Health)
            .Produces<object>(StatusCodes.Status200OK);

        group.MapPost("getserviceinfo", GetServiceInfo)
            .Produces<object>(StatusCodes.Status200OK);

        return app;
    }

    private static IResult Ping()
    {
        return Results.Ok(new PingResult(AuthConstants.ModuleId, "System"));
    }

    private static async Task<IResult> Health(ISystemService systemService)
    {
        var healthStatus = await systemService.Health();
        return Results.Ok(healthStatus);
    }

    private static async Task<IResult> GetServiceInfo(ISystemService systemService)
    {
        var serviceInfo = await systemService.GetServiceInfo();
        return Results.Ok(serviceInfo);
    }
}
