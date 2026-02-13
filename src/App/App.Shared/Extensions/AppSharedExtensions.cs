using Dyvenix.App1.App.Shared.ApiClients;
using Dyvenix.App1.App.Shared.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dyvenix.App1.App.Shared.Extensions;

public static class AppSharedExtensions
{
	public static IServiceCollection AddAppSharedServices(this IServiceCollection services, IConfiguration configuration, bool inProcess)
	{
		var serviceConfig = configuration.GetSection("ApiClients:AppBaseUrl");

		if (!inProcess)
		{
			// Read configuration
			string? baseUrl = serviceConfig.GetValue<string>("BaseUrl");
			if (string.IsNullOrEmpty(baseUrl))
			{
				throw new InvalidOperationException(
					"ServiceClients:App1:BaseUrl is required when InProcess is false");
			}

			services.AddHttpClient<IAppSystemService, SystemApiClient>(client =>
			{
				client.BaseAddress = new Uri(baseUrl);
			});
		}

		return services;
	}
}
