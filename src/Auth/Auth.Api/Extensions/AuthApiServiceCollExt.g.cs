//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/15/2026 7:07 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Auth.Endpoints.v1;
using Dyvenix.App1.Common.Api.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using cv1 = Dyvenix.App1.Auth.Shared.Contracts.v1;
using sv1 = Dyvenix.App1.Auth.Api.Services.v1;

namespace Dyvenix.App1.Auth.Api.Extensions;

public static partial class AuthApiServiceCollExt
{
	static partial void AddGeneratedServices(IServiceCollection services)
	{
		// AppUserService
		services.AddScoped<cv1.IAppUserService, sv1.AppUserService>();
		services.AddScoped<ApiExceptionFilter< sv1.AppUserService>>();
	}
	
	private static partial void MapGeneratedEndpoints(IEndpointRouteBuilder app)
	{
		app.MapAppUserEndpoints();
	}
}
