//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/14/2026 11:12 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Dyvenix.App1.App.Shared.ApiClients;
using Dyvenix.App1.App.Shared.Contracts;
using sv1 = Dyvenix.App1.App.Shared.ApiClients.v1;
using cv1 = Dyvenix.App1.App.Shared.Contracts.v1;

namespace Dyvenix.App1.App.Shared.Extensions;

public static partial class AppSharedServiceCollExt
{
	static partial void AddGeneratedServices(IServiceCollection services)
	{
		services.AddHttpClient<IAppSystemService, SystemApiClient>();
		
		// PatientService
		services.AddHttpClient<cv1.IPatientService, sv1.PatientApiClient>();
		// InvoiceService
		services.AddHttpClient<cv1.IInvoiceService, sv1.InvoiceApiClient>();
	}
}
