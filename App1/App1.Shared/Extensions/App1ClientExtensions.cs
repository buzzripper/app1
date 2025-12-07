using Dyvenix.App1.Shared.Interfaces;
using Dyvenix.App1.Shared.Proxies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dyvenix.App1.Shared.Extensions;

public static class App1ClientExtensions
{
	public static IServiceCollection AddApp1Client(this IServiceCollection services, IConfiguration configuration, bool inProcess)
	{
		var serviceConfig = configuration.GetSection("ServiceClients:App1");

		if (!inProcess)
		{
			// HTTP: Use HTTP client proxy
			string? baseUrl = serviceConfig.GetValue<string>("Url");
			if (string.IsNullOrEmpty(baseUrl))
			{
				throw new InvalidOperationException(
					"ServiceClients:App1:Url is required when InProcess is false");
			}

			services.AddHttpClient<ISystemService, SystemServiceHttpClient>(client =>
			{
				client.BaseAddress = new Uri(baseUrl);
			});
		}
		// Note: For in-process mode, the consumer must register ISystemService implementation
		// by calling AddApp1ApiServices() from App1.Api before calling AddApp1Client()

		return services;
	}
}
