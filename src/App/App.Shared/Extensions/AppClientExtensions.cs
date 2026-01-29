using App1.App.Shared.Interfaces;
using App1.App.Shared.Proxies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App1.App.Shared.Extensions;

public static class AppClientExtensions
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

			services.AddHttpClient<IAppSystemService, SystemServiceHttpClient>(client =>
			{
				client.BaseAddress = new Uri(baseUrl);
			});
		}

		return services;
	}
}
