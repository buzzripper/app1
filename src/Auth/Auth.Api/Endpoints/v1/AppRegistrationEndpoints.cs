using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.DTOs;

namespace Dyvenix.App1.Auth.Endpoints.v1;

public static class AppRegistrationEndpoints
{
    public static IEndpointRouteBuilder MapAppRegistrationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth/v1/appregistration")
            .WithTags("AppRegistration");

        group.MapGet("GetById/{id}", GetById)
            .Produces<AppRegistrationDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("GetByClientId/{clientId}", GetByClientId)
            .Produces<AppRegistrationDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("GetAll", GetAll)
            .Produces<IReadOnlyList<AppRegistrationDto>>(StatusCodes.Status200OK);

        group.MapPost("Create", Create)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("Update", Update)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("Delete/{id}", Delete)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }

    public static async Task<Result<AppRegistrationDto?>> GetById(IAppRegistrationService appRegistrationService, string id)
    {
        var dto = await appRegistrationService.GetById(id);
        return Result<AppRegistrationDto?>.Ok(dto);
    }

    public static async Task<Result<AppRegistrationDto?>> GetByClientId(IAppRegistrationService appRegistrationService, string clientId)
    {
        var dto = await appRegistrationService.GetByClientId(Uri.UnescapeDataString(clientId));
        return Result<AppRegistrationDto?>.Ok(dto);
    }

    public static async Task<Result<IReadOnlyList<AppRegistrationDto>>> GetAll(IAppRegistrationService appRegistrationService)
    {
        var data = await appRegistrationService.GetAll();
        return Result<IReadOnlyList<AppRegistrationDto>>.Ok(data);
    }

    public static async Task<Result<string>> Create(IAppRegistrationService appRegistrationService, [FromBody] CreateAppRegistrationReq request)
    {
        var id = await appRegistrationService.Create(request);
        return Result<string>.Ok(id);
    }

    public static async Task<Result> Update(IAppRegistrationService appRegistrationService, [FromBody] UpdateAppRegistrationReq request)
    {
        await appRegistrationService.Update(request);
        return Result.Ok();
    }

    public static async Task<Result> Delete(IAppRegistrationService appRegistrationService, string id)
    {
        await appRegistrationService.Delete(id);
        return Result.Ok();
    }
}
