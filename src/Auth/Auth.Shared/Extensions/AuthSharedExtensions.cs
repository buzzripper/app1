using Dyvenix.App1.Auth.Shared.Interfaces;
using Dyvenix.App1.Auth.Shared.Proxies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dyvenix.App1.Auth.Shared.Extensions;

public static class AuthSharedExtensions
{
	public static IServiceCollection AddAuthSharedServices(this IServiceCollection services, IConfiguration configuration, bool inProcess)
	{
		var serviceConfig = configuration.GetSection("ApiClients:Auth");

		if (!inProcess)
		{
			// HTTP: Use HTTP client proxy
			string? baseUrl = serviceConfig.GetValue<string>("BaseUrl");
			if (string.IsNullOrEmpty(baseUrl))
			{
				throw new InvalidOperationException(
					"ApiClients:Auth:BaseUrl is required when InProcess is false");
			}

			services.AddHttpClient<IAuthSystemService, SystemServiceHttpClient>(client =>
			{
				client.BaseAddress = new Uri(baseUrl);
			});
		}

		return services;
	}
}
