using Dyvenix.App1.App.Shared.Authorization;
using Dyvenix.App1.Common.Shared.Contracts;
using Dyvenix.App1.Common.Shared.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using System.Text;

namespace Dyvenix.App1.App.Api.Endpoints;

public static class AppSystemEndpoints
{
    public static IEndpointRouteBuilder MapAppSystemEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/app/system")
            .WithTags("System");

        group.MapGet("ping", Ping)
            .Produces<PingResult>(StatusCodes.Status200OK)
            .AllowAnonymous();

        group.MapGet("health", Health)
            .Produces<object>(StatusCodes.Status200OK)
            .RequireAuthorization(AppPermissions.Write);

        group.MapPost("getserviceinfo", GetServiceInfo)
            .Produces<object>(StatusCodes.Status200OK)
            .AllowAnonymous();

        return app;
    }

    private static IResult Ping(ClaimsPrincipal user)
    {
        var sb = new StringBuilder();
        foreach (var claim in user.Claims.Select(c => $"{c.Type} = {c.Value}"))
            sb.AppendLine(claim);
        File.WriteAllText(@"D:\Active\Claims.txt", sb.ToString());

        return Results.Ok(new PingResult(AppConstants.ModuleId, "System"));
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
