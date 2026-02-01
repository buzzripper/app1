//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/1/2026 4:43 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Api.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Dyvenix.App1.App.Api.Config;

public static partial class ServiceCollectionExt
{
	static partial void AddGeneratedServices(IServiceCollection services)
	{
		services.AddScoped<Dyvenix.App1.App.Api.Services.v1.IPatientService, Api.Services.v1.PatientService>();
		services.AddScoped<ApiExceptionFilter<Dyvenix.App1.App.Api.Services.v1.PatientService>>();
	}
}
