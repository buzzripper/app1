using Auth.Shared.Interfaces;
using Auth.Shared.Proxies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Shared.Extensions;

public static class AuthClientExtensions
{
    public static IServiceCollection AddAuthClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var serviceConfig = configuration.GetSection("ServiceClients:Auth");
        bool inProcess = serviceConfig.GetValue<bool>("InProcess", false);
        
        if (!inProcess)
        {
            // HTTP: Use HTTP client proxy
            string? baseUrl = serviceConfig.GetValue<string>("Url");
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException(
                    "ServiceClients:Auth:Url is required when InProcess is false");
            }
            
            services.AddHttpClient<ISystemService, SystemServiceHttpClient>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });
        }
        // Note: For in-process mode, the consumer must register ISystemService implementation
        // by calling AddAuthApiServices() from Auth.Api before calling AddAuthClient()
        
        return services;
    }
}
