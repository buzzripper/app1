using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Dyvenix.App1.Common.Api.Filters;
using sv1 = Dyvenix.App1.AdAgent.Api.Services.v1;
using cv1 = Dyvenix.App1.AdAgent.Shared.Contracts.v1;
using Dyvenix.App1.AdAgent.Api.Endpoints;
using Dyvenix.App1.AdAgent.Api.Endpoints.v1;

namespace Dyvenix.App1.AdAgent.Api.Extensions;

public static partial class AdAgentApiServiceCollExt
{
	public static partial void AddGeneratedServices(IServiceCollection services)
	{
		// AdService
		services.AddScoped<cv1.IAdService, sv1.AdService>();
		services.AddScoped<ApiExceptionFilter<sv1.AdService>>();
	}

	public static partial void MapGeneratedEndpoints(IEndpointRouteBuilder app)
	{
		app.MapAdEndpoints();
		ServiceRuntime.Start();
	}
}
