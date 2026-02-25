//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/18/2026 7:27 AM. Any changes made to it will be lost.
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
	public static partial void AddGeneratedServices(IServiceCollection services)
	{
		// PatientService
		services.AddScoped<cv1.IPatientService, sv1.PatientService>();
		services.AddScoped<ApiExceptionFilter<sv1.PatientService>>();
		// InvoiceService
		services.AddScoped<cv1.IInvoiceService, sv1.InvoiceService>();
		services.AddScoped<ApiExceptionFilter<sv1.InvoiceService>>();
	}

	public static partial void MapGeneratedEndpoints(IEndpointRouteBuilder app)
	{
		app.MapPatientEndpoints();
		app.MapInvoiceEndpoints();

		ServiceRuntime.Start();
	}
}
