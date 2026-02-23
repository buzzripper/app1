using Fido2Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OpeniddictServer.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly ITenantContext? _tenantContext;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantContext? tenantContext = null)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    public DbSet<FidoStoredCredential> FidoStoredCredential => Set<FidoStoredCredential>();

    public DbSet<Tenant> Tenants => Set<Tenant>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<FidoStoredCredential>().HasKey(m => m.Id);

        builder.Entity<Tenant>(b =>
        {
            b.HasKey(t => t.Id);
            b.HasIndex(t => t.Slug).IsUnique();
            b.Property(t => t.Name).HasMaxLength(200).IsRequired();
            b.Property(t => t.Slug).HasMaxLength(100).IsRequired();
            b.Property(t => t.AuthMethod).HasMaxLength(50).IsRequired();
            b.Property(t => t.ExternalAuthority).HasMaxLength(500);
            b.Property(t => t.ExternalClientId).HasMaxLength(200);
            b.Property(t => t.ExternalClientSecret).HasMaxLength(500);
        });

        builder.Entity<ApplicationUser>(b =>
        {
            b.HasIndex(u => new { u.TenantId, u.Email });
            b.HasOne<Tenant>().WithMany().HasForeignKey(u => u.TenantId).OnDelete(DeleteBehavior.Restrict);

            // Global query filter: all Identity queries are scoped to the current tenant
            b.HasQueryFilter(u => _tenantContext == null
                || _tenantContext.TenantId == Guid.Empty
                || u.TenantId == _tenantContext.TenantId);
        });

        base.OnModelCreating(builder);
    }
}