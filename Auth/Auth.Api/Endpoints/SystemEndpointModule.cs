using Auth.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Auth.Api.Endpoints;

public class SystemEndpointModule : IEndpointModule
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth/system").WithTags("System");
        
        group.MapGet("/check-health", CheckHealth).WithName("SystemCheckHealth").AllowAnonymous();
    }

    [AllowAnonymous]
    private static async Task<IResult> CheckHealth(ISystemService systemService)
    {
        var health = await systemService.CheckHealth();
        return Results.Ok(health);
    }
}
