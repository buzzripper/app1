//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/13/2026 8:31 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Dyvenix.App1.Common.Api.Filters;
using sv1 = Dyvenix.App1.App.Api.Services.v1;
using cv1 = Dyvenix.App1.App.Shared.Contracts.v1;

namespace Dyvenix.App1.App.Api.Extensions;

public static partial class ServiceCollectionExt
{
	static partial void AddGeneratedServices(IServiceCollection services)
	{
		// PatientService
		services.AddScoped<cv1.IPatientService, sv1.PatientService>();
		services.AddScoped<ApiExceptionFilter< sv1.PatientService>>();
		// InvoiceService
		services.AddScoped<cv1.IInvoiceService, sv1.InvoiceService>();
		services.AddScoped<ApiExceptionFilter< sv1.InvoiceService>>();
	}
}
