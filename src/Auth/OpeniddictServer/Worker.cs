using OpenIddict.Abstractions;
using OpeniddictServer.Data;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace OpeniddictServer
{
    public class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            await RegisterApplicationsAsync(scope.ServiceProvider);
            await RegisterScopesAsync(scope.ServiceProvider);

            static async Task RegisterApplicationsAsync(IServiceProvider provider)
            {
                var manager = provider.GetRequiredService<IOpenIddictApplicationManager>();

                // Portal BFF (confidential client, authorization code + refresh token)
                if (await manager.FindByClientIdAsync("portal-bff") is null)
                {
                    await manager.CreateAsync(new OpenIddictApplicationDescriptor
                    {
                        ClientId = "portal-bff",
                        ClientSecret = "portal-bff-secret",
                        ConsentType = ConsentTypes.Implicit,
                        DisplayName = "Portal BFF",
                        PostLogoutRedirectUris =
                        {
                            new Uri("https://localhost:5001/signout-callback-oidc")
                        },
                        RedirectUris =
                        {
                            new Uri("https://localhost:5001/signin-oidc")
                        },
                        Permissions =
                        {
                            Permissions.Endpoints.Authorization,
                            Permissions.Endpoints.EndSession,
                            Permissions.Endpoints.Token,
                            Permissions.Endpoints.Revocation,
                            Permissions.GrantTypes.AuthorizationCode,
                            Permissions.GrantTypes.RefreshToken,
                            Permissions.ResponseTypes.Code,
                            Permissions.Scopes.Email,
                            Permissions.Scopes.Profile,
                            Permissions.Scopes.Roles,
                            Permissions.Prefixes.Scope + "app1-api"
                        },
                        Requirements =
                        {
                            Requirements.Features.ProofKeyForCodeExchange
                        }
                    });
                }

                // Client credentials (for future machine-to-machine / partner integrations)
                if (await manager.FindByClientIdAsync("CC") is null)
                {
                    await manager.CreateAsync(new OpenIddictApplicationDescriptor
                    {
                        ClientId = "CC",
                        ClientSecret = "cc_secret",
                        DisplayName = "CC for protected API",
                        Permissions =
                        {
                            Permissions.Endpoints.Authorization,
                            Permissions.Endpoints.Token,
                            Permissions.GrantTypes.ClientCredentials,
                            Permissions.Prefixes.Scope + "app1-api"
                        }
                    });
                }

                // API resource server (for introspection if needed)
                if (await manager.FindByClientIdAsync("app1Apis") is null)
                {
                    await manager.CreateAsync(new OpenIddictApplicationDescriptor
                    {
                        ClientId = "app1Apis",
                        ClientSecret = "app1Apis-secret",
                        Permissions =
                        {
                            Permissions.Endpoints.Introspection
                        }
                    });
                }
            }

            static async Task RegisterScopesAsync(IServiceProvider provider)
            {
                var manager = provider.GetRequiredService<IOpenIddictScopeManager>();

                if (await manager.FindByNameAsync("app1-api") is null)
                {
                    await manager.CreateAsync(new OpenIddictScopeDescriptor
                    {
                        DisplayName = "App1 API access",
                        Name = "app1-api",
                        Resources =
                        {
                            "app1Apis"
                        }
                    });
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
