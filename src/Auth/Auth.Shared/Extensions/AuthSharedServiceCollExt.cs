using Dyvenix.App1.Auth.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.Config;
using Dyvenix.App1.Common.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Dyvenix.App1.Auth.Shared.Extensions;

public static partial class AuthSharedServiceCollExt
{
    // Declaration of partial method for code-generated services
    static partial void AddGeneratedServices(IServiceCollection services);

    public static IServiceCollection AddAuthSharedServices(this IServiceCollection services, ApiClientConfig apiClientConfig, bool inProcess)
    {
        if (!inProcess)
        {
            string? baseUrl = apiClientConfig.BaseUrl;
            if (string.IsNullOrEmpty(apiClientConfig.BaseUrl))
            {
                throw new InvalidOperationException(
                    "BaseUrl is missing from Auth configuration. It is required when InProcess is false");
            }

            services.AddHttpClient<ISystemService, AuthSystemApiClient>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

            // Add code-generated services
            AddGeneratedServices(services);
        }

        return services;
    }
}
