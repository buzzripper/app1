using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            await SeedTenantsAsync(context);
            await SeedTestUsersAsync(scope.ServiceProvider);
            await RegisterApplicationsAsync(scope.ServiceProvider);
            await RegisterScopesAsync(scope.ServiceProvider);

            static async Task RegisterApplicationsAsync(IServiceProvider provider)
            {
                var manager = provider.GetRequiredService<IOpenIddictApplicationManager>();

                // Portal BFF (confidential client, authorization code + refresh token)
                // Delete and recreate to ensure redirect URIs stay in sync with tenant list.
                var portalBff = await manager.FindByClientIdAsync("portal-bff");
                if (portalBff != null)
                {
                    await manager.DeleteAsync(portalBff);
                }

                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "portal-bff",
                    ClientSecret = "portal-bff-secret",
                    ConsentType = ConsentTypes.Implicit,
                    DisplayName = "Portal BFF",
                    PostLogoutRedirectUris =
                    {
                        new Uri("https://localhost:5001/signout-callback-oidc"),
                        new Uri("https://acme.localhost:5001/signout-callback-oidc"),
                        new Uri("https://contoso.localhost:5001/signout-callback-oidc")
                    },
                    RedirectUris =
                    {
                        new Uri("https://localhost:5001/signin-oidc"),
                        new Uri("https://acme.localhost:5001/signin-oidc"),
                        new Uri("https://contoso.localhost:5001/signin-oidc")
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

            static async Task SeedTenantsAsync(ApplicationDbContext context)
            {
                var acmeId = Guid.Parse("A1000000-0000-0000-0000-000000000001");
                var contosoId = Guid.Parse("A1000000-0000-0000-0000-000000000002");

                if (!await context.Tenants.AnyAsync(t => t.Id == acmeId))
                {
                    context.Tenants.Add(new Tenant
                    {
                        Id = acmeId,
                        Name = "Acme Corp",
                        Slug = "acme",
                        AuthMethod = "Local"
                    });
                }

                if (!await context.Tenants.AnyAsync(t => t.Id == contosoId))
                {
                    context.Tenants.Add(new Tenant
                    {
                        Id = contosoId,
                        Name = "Contoso",
                        Slug = "contoso",
                        AuthMethod = "Local"
                    });
                }

                await context.SaveChangesAsync();
            }

            static async Task SeedTestUsersAsync(IServiceProvider provider)
            {
                var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();

                var testUsers = new[]
                {
                    new { Email = "acme@test.com", TenantId = Guid.Parse("A1000000-0000-0000-0000-000000000001") },
                    new { Email = "contoso@test.com", TenantId = Guid.Parse("A1000000-0000-0000-0000-000000000002") }
                };

                foreach (var testUser in testUsers)
                {
                    if (await userManager.FindByIdAsync(testUser.TenantId.ToString()) is null
                        && (await userManager.FindByEmailAsync(testUser.Email)) is null)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = testUser.Email,
                            Email = testUser.Email,
                            TenantId = testUser.TenantId,
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(user, "Test1234!");
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
