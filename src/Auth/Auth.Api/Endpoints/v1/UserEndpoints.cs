using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.DTOs;

namespace Dyvenix.App1.Auth.Endpoints.v1;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth/v1/user")
            .WithTags("User");

        group.MapGet("GetById/{id}", GetById)
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("GetByEmail/{email}", GetByEmail)
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("GetAllByTenant/{tenantId}", GetAllByTenant)
            .Produces<IReadOnlyList<UserSummaryDto>>(StatusCodes.Status200OK);

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

    public static async Task<Result<UserDto?>> GetById(IUserService userService, string id)
    {
        var dto = await userService.GetById(id);
        return Result<UserDto?>.Ok(dto);
    }

    public static async Task<Result<UserDto?>> GetByEmail(IUserService userService, string email)
    {
        var dto = await userService.GetByEmail(Uri.UnescapeDataString(email));
        return Result<UserDto?>.Ok(dto);
    }

    public static async Task<Result<IReadOnlyList<UserSummaryDto>>> GetAllByTenant(IUserService userService, Guid tenantId)
    {
        var data = await userService.GetAllByTenant(tenantId);
        return Result<IReadOnlyList<UserSummaryDto>>.Ok(data);
    }

    public static async Task<Result<string>> Create(IUserService userService, [FromBody] CreateUserReq request)
    {
        var id = await userService.Create(request);
        return Result<string>.Ok(id);
    }

    public static async Task<Result> Update(IUserService userService, [FromBody] UpdateUserReq request)
    {
        await userService.Update(request);
        return Result.Ok();
    }

    public static async Task<Result> Delete(IUserService userService, string id)
    {
        await userService.Delete(id);
        return Result.Ok();
    }
}
