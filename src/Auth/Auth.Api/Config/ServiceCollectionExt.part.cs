//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 1/31/2026 2:55 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Dyvenix.App1.Auth.Api.Filters;
using Dyvenix.App1.Auth.Api.Services.v1;

namespace Dyvenix.App1.Auth.Api.Config;

public static partial class ServiceCollectionExt
{
	static partial void AddGeneratedServices(IServiceCollection services)
	{
		services.AddScoped<Dyvenix.App1.Auth.Api.Services.v1.IPersonService, Dyvenix.App1.Auth.Api.Services.v1.PersonService>();
		services.AddScoped<AuthExceptionFilter<Dyvenix.App1.Auth.Api.Services.v1.IPersonService>>();
		services.AddScoped<ILogger<PersonService>>();
	}
}
