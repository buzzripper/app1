//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/27/2026 4:53 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Dyvenix.App1.Common.Api.Filters;
using sv1 = Dyvenix.App1.Auth.Api.Services.v1;
using cv1 = Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Endpoints.v1;

namespace Dyvenix.App1.Auth.Api.Extensions;

public static partial class AuthApiServiceCollExt
{
	public static partial void AddGeneratedServices(IServiceCollection services)
	{
		// TenantService
		services.AddScoped<cv1.ITenantService, sv1.TenantService>();
		services.AddScoped<ApiExceptionFilter<sv1.TenantService>>();
	}

	public static partial void MapGeneratedEndpoints(IEndpointRouteBuilder app)
	{
		app.MapTenantEndpoints();
	}
}
