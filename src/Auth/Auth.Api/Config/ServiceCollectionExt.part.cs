//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/10/2026 9:14 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Auth.Services.v1;

namespace Dyvenix.App1.Auth.Api.Config;

public static partial class ServiceCollectionExt
{
	static partial void AddGeneratedServices(IServiceCollection services)
	{
		services.AddScoped<Dyvenix.App1.Auth.Services.v1.IAppUserService, Dyvenix.App1.Auth.Services.v1.AppUserService>();
		services.AddScoped<ApiExceptionFilter<Dyvenix.App1.Auth.Services.v1.AppUserService>>();
	}
}
