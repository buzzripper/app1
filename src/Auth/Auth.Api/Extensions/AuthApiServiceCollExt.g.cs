//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/14/2026 2:39 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Dyvenix.App1.Common.Api.Filters;
using sv1 = Dyvenix.App1.Auth.Api.Services.v1;
using cv1 = Dyvenix.App1.Auth.Shared.Contracts.v1;

namespace Dyvenix.App1.Auth.Api.Extensions;

public static partial class AuthApiServiceCollExt
{
	static partial void AddGeneratedServices(IServiceCollection services)
	{
		// AppUserService
		services.AddScoped<cv1.IAppUserService, sv1.AppUserService>();
		services.AddScoped<ApiExceptionFilter< sv1.AppUserService>>();
	}
}
