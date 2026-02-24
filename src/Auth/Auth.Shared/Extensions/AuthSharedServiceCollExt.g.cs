//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/18/2026 7:27 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Dyvenix.App1.Auth.Shared.ApiClients;
using Dyvenix.App1.Auth.Shared.Contracts;

namespace Dyvenix.App1.Auth.Shared.Extensions;

public static partial class AuthSharedServiceCollExt
{
	static partial void AddGeneratedServices(IServiceCollection services)
	{
		services.AddHttpClient<IAuthSystemService, AuthSystemApiClient>();
	}
}
