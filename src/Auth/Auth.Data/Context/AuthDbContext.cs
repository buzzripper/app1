using Dyvenix.App1.Auth.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dyvenix.App1.Auth.Data.Context;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
	private readonly ITenantContext? _tenantContext;

	/// <summary>
	/// Constructor for direct use (no derived context).
	/// </summary>
	public AuthDbContext(DbContextOptions<AuthDbContext> options, ITenantContext? tenantContext = null)
		: base(options)
	{
		_tenantContext = tenantContext;
	}

	/// <summary>
	/// Constructor for derived contexts (e.g. AuthServerDbContext in Auth.Server).
	/// </summary>
	protected AuthDbContext(DbContextOptions options, ITenantContext? tenantContext = null)
		: base(options)
	{
		_tenantContext = tenantContext;
	}

	public DbSet<Tenant> Tenant => Set<Tenant>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<Tenant>(b =>
		{
			b.HasKey(t => t.Id);
			b.HasIndex(t => t.Slug).IsUnique();
			b.Property(t => t.Name).HasMaxLength(200).IsRequired();
			b.Property(t => t.Slug).HasMaxLength(100).IsRequired();
			b.Property(t => t.AuthMode).IsRequired();
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
