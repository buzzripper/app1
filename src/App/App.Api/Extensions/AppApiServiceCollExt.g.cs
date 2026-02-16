//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/15/2026 7:07 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.App.Endpoints.v1;
using Dyvenix.App1.Common.Api.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using cv1 = Dyvenix.App1.App.Shared.Contracts.v1;
using sv1 = Dyvenix.App1.App.Api.Services.v1;

namespace Dyvenix.App1.App.Api.Extensions;

public static partial class AppApiServiceCollExt
{
	private static partial void AddGeneratedServices(IServiceCollection services)
	{
		// PatientService
		services.AddScoped<cv1.IPatientService, sv1.PatientService>();
		services.AddScoped<ApiExceptionFilter< sv1.PatientService>>();
		// InvoiceService
		services.AddScoped<cv1.IInvoiceService, sv1.InvoiceService>();
		services.AddScoped<ApiExceptionFilter< sv1.InvoiceService>>();
	}

	private static partial void MapGeneratedEndpoints(IEndpointRouteBuilder app)
	{
		app.MapPatientEndpoints();
		app.MapInvoiceEndpoints();
	}
}
