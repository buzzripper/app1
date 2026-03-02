using Dyvenix.App1.AdAgent.Shared.Contracts.v1;
using Dyvenix.App1.AdAgent.Shared.DTOs;
using Dyvenix.App1.AdAgent.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Dyvenix.App1.AdAgent.Api.Endpoints.v1;

public static class AdEndpoints
{
	public static IEndpointRouteBuilder MapAdEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/adagent/v1/ad")
			.WithTags("AdService");

		group.MapPost("AuthenticateUser", AuthenticateUser)
			.Produces<Guid>(StatusCodes.Status200OK);

		return app;
	}

	public static async Task<Result<AdAuthResult>> AuthenticateUser(IAdService adService, AuthenticateUserReq request)
	{
		var result = await adService.AuthenticateUser(request.UserUpnOrDomainUser, request.Password);
		return Result<AdAuthResult>.Ok(result);
	}

}
