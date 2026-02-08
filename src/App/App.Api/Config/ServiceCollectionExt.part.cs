//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/7/2026 9:13 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.App.Services.v1;

namespace Dyvenix.App1.App.Api.Config;

public static partial class ServiceCollectionExt
{
	static partial void AddGeneratedServices(IServiceCollection services)
	{
		services.AddScoped<Dyvenix.App1.App.Services.v1.IPatientService, Dyvenix.App1.App.Services.v1.PatientService>();
		services.AddScoped<ApiExceptionFilter<Dyvenix.App1.App.Services.v1.PatientService>>();
		services.AddScoped<Dyvenix.App1.App.Services.v1.IInvoiceService, Dyvenix.App1.App.Services.v1.InvoiceService>();
		services.AddScoped<ApiExceptionFilter<Dyvenix.App1.App.Services.v1.InvoiceService>>();
	}
}
