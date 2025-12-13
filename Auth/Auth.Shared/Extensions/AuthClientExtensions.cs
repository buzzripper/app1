using Dyvenix.Auth.Shared.Interfaces;
using Dyvenix.Auth.Shared.Proxies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dyvenix.Auth.Shared.Extensions;

public static class AuthClientExtensions
{
	public static IServiceCollection AddAuthClients(this IServiceCollection services, IConfiguration configuration, bool inProcess)
	{
		var serviceConfig = configuration.GetSection("ServiceClients:Auth");

		if (!inProcess)
		{
			// HTTP: Use HTTP client proxy
			string? baseUrl = serviceConfig.GetValue<string>("Url");
			if (string.IsNullOrEmpty(baseUrl))
			{
				throw new InvalidOperationException(
					"ServiceClients:Auth:Url is required when InProcess is false");
			}

			services.AddHttpClient<IAuthSystemService, SystemServiceHttpClient>(client =>
			{
				client.BaseAddress = new Uri(baseUrl);
			});
		}

		return services;
	}
}
