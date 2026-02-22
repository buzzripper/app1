using Dyvenix.App1.Common.Api.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Dyvenix.App1.Common.Api.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        return services;
    }

    public static IServiceCollection AddTestJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(TestJwtAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, TestJwtAuthenticationHandler>(
                TestJwtAuthenticationHandler.SchemeName, _ => { });

        return services;
    }
}
