using Dyvenix.App1.Auth.Data;
using Dyvenix.App1.Auth.Data.Context;
using Dyvenix.App1.Auth.Server.Fido2;
using Microsoft.EntityFrameworkCore;

namespace Dyvenix.App1.Auth.Server.Data;

/// <summary>
/// Auth.Server-specific DbContext that extends AuthDbContext with
/// server-only entities (e.g. FIDO2/WebAuthn credentials).
/// </summary>
public class AuthServerDbContext(DbContextOptions<AuthServerDbContext> options, ITenantContext? tenantContext = null)
    : AuthDbContext(options, tenantContext)
{
    public DbSet<FidoStoredCredential> FidoStoredCredential => Set<FidoStoredCredential>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<FidoStoredCredential>().HasKey(m => m.Id);
    }
}
