//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/8/2026 11:54 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Dyvenix.App1.Common.Api.Filters;
using sv1 = Dyvenix.App1.App.Api.Services.v1;
using cv1 = Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Endpoints.v1;

namespace Dyvenix.App1.App.Api.Extensions;

public static partial class AppApiServiceCollExt
{
	static partial void AddGeneratedServices(IServiceCollection services)
	{
		// ClientService
		services.AddScoped<cv1.IClientService, sv1.ClientService>();
		services.AddScoped<ApiExceptionFilter<sv1.ClientService>>();
	}

	static partial void MapGeneratedEndpoints(IEndpointRouteBuilder app)
	{
		app.MapClientEndpoints();
	}
}
