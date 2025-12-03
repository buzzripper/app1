using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpeniddictServer.Data;
using System.Globalization;
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

            await RegisterRolesAndUsersAsync(scope.ServiceProvider);
            await RegisterApplicationsAsync(scope.ServiceProvider);
            await RegisterScopesAsync(scope.ServiceProvider);

            static async Task RegisterRolesAndUsersAsync(IServiceProvider provider)
            {
                var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
                
                // Create roles if they don't exist
                if (!await roleManager.RoleExistsAsync("dataEventRecords.user"))
                {
                    await roleManager.CreateAsync(new IdentityRole("dataEventRecords.user"));
                }
                
                if (!await roleManager.RoleExistsAsync("dataEventRecords.admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("dataEventRecords.admin"));
                }
            }

            static async Task RegisterApplicationsAsync(IServiceProvider provider)
            {
                var manager = provider.GetRequiredService<IOpenIddictApplicationManager>();

                // Portal.Server BFF client
                if (await manager.FindByClientIdAsync("portalclient") is null)
                {
                    await manager.CreateAsync(new OpenIddictApplicationDescriptor
                    {
                        ClientId = "portalclient",
                        ClientSecret = "portal-secret-change-in-production",
                        ConsentType = ConsentTypes.Explicit,
                        DisplayName = "Portal Server BFF",
                        PostLogoutRedirectUris =
                        {
                            new Uri("https://localhost:5001/signout-callback-oidc"),
                            new Uri("https://localhost:5001/")
                        },
                        RedirectUris =
                        {
                            new Uri("https://localhost:5001/signin-oidc"),
                            new Uri("https://localhost:5001/")
                        },
                        Permissions =
                        {
                            Permissions.Endpoints.Authorization,
                            Permissions.Endpoints.EndSession,
                            Permissions.Endpoints.Token,
                            Permissions.Endpoints.Revocation,
                            Permissions.Endpoints.Introspection,
                            Permissions.GrantTypes.AuthorizationCode,
                            Permissions.GrantTypes.RefreshToken,
                            Permissions.ResponseTypes.Code,
                            Permissions.Scopes.Email,
                            Permissions.Scopes.Profile,
                            Permissions.Scopes.Roles,
                            Permissions.Prefixes.Scope + "dataEventRecords",
                            Permissions.Prefixes.Scope + "offline_access"
                        },
                        Requirements =
                        {
                            Requirements.Features.ProofKeyForCodeExchange
                        }
                    });
                }

                // Angular UI client
                if (await manager.FindByClientIdAsync("angularclient") is null)
                {
                    await manager.CreateAsync(new OpenIddictApplicationDescriptor
                    {
                        ClientId = "angularclient",
                        ConsentType = ConsentTypes.Explicit,
                        DisplayName = "angular client PKCE",
                        DisplayNames =
                        {
                            [CultureInfo.GetCultureInfo("fr-FR")] = "Application cliente MVC"
                        },
                        PostLogoutRedirectUris =
                        {
                            new Uri("https://localhost:4200")
                        },
                        RedirectUris =
                        {
                            new Uri("https://localhost:4200")
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
                            Permissions.Prefixes.Scope + "dataEventRecords"
                        },
                        Requirements =
                        {
                            Requirements.Features.ProofKeyForCodeExchange
                        }
                    });
                }

                // API application CC
                if (await manager.FindByClientIdAsync("CC") == null)
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
                        Permissions.Prefixes.Scope + "dataEventRecords"
                    }
                    });
                }

                // API
                if (await manager.FindByClientIdAsync("rs_dataEventRecordsApi") == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "rs_dataEventRecordsApi",
                        ClientSecret = "dataEventRecordsSecret",
                        Permissions =
                        {
                            Permissions.Endpoints.Introspection
                        }
                    };

                    await manager.CreateAsync(descriptor);
                }

                // Blazor Hosted
                if (await manager.FindByClientIdAsync("blazorcodeflowpkceclient") is null)
                {
                    await manager.CreateAsync(new OpenIddictApplicationDescriptor
                    {
                        ClientId = "blazorcodeflowpkceclient",
                        ConsentType = ConsentTypes.Explicit,
                        DisplayName = "Blazor code PKCE",
                        DisplayNames =
                        {
                            [CultureInfo.GetCultureInfo("fr-FR")] = "Application cliente MVC"
                        },
                        PostLogoutRedirectUris =
                        {
                            new Uri("https://localhost:44348/signout-callback-oidc"),
                            new Uri("https://localhost:5001/signout-callback-oidc")
                        },
                        RedirectUris =
                        {
                            new Uri("https://localhost:44348/signin-oidc"),
                            new Uri("https://localhost:5001/signin-oidc")
                        },
                        ClientSecret = "codeflow_pkce_client_secret",
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
                            Permissions.Prefixes.Scope + "dataEventRecords"
                        },
                        Requirements =
                        {
                            Requirements.Features.ProofKeyForCodeExchange
                        }
                    });
                }
            }

            static async Task RegisterScopesAsync(IServiceProvider provider)
            {
                var manager = provider.GetRequiredService<IOpenIddictScopeManager>();

                if (await manager.FindByNameAsync("dataEventRecords") is null)
                {
                    await manager.CreateAsync(new OpenIddictScopeDescriptor
                    {
                        DisplayName = "dataEventRecords API access",
                        DisplayNames =
                        {
                            [CultureInfo.GetCultureInfo("fr-FR")] = "Accès à l'API de démo"
                        },
                        Name = "dataEventRecords",
                        Resources =
                        {
                            "rs_dataEventRecordsApi"
                        }
                    });
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
