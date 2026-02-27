//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/27/2026 4:53 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.Contracts;
using Dyvenix.App1.Auth.Shared.ApiClients;
using Dyvenix.App1.Auth.Shared.Contracts;
using sv1 = Dyvenix.App1.Auth.Shared.ApiClients.v1;
using cv1 = Dyvenix.App1.Auth.Shared.Contracts.v1;

namespace Dyvenix.App1.Auth.Shared.Extensions;

public static partial class AuthSharedServiceCollExt
{
	static partial void AddGeneratedServices(IServiceCollection services)
	{
		services.AddHttpClient<ISystemService, AuthSystemApiClient>();
		
		// TenantService
		services.AddHttpClient<cv1.ITenantService, sv1.TenantApiClient>();
	}
}
