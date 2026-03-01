using Dyvenix.App1.Auth.Data.Entities;
using Dyvenix.App1.Auth.Data.Fido2;
using Microsoft.EntityFrameworkCore;

namespace Dyvenix.App1.Auth.Data.Context;

public partial class AuthDbContext
{
	private readonly ITenantContext? _tenantContext;

	/// <summary>
	/// Constructor for derived contexts (e.g. AuthServerDbContext in Auth.Server).
	/// </summary>
	public AuthDbContext(DbContextOptions<AuthDbContext> options, ITenantContext? tenantContext = null)
		: base(options)
	{
		_tenantContext = tenantContext;
	}

	public DbSet<FidoStoredCredential> FidoStoredCredential => Set<FidoStoredCredential>();

	partial void OnModelCreatingExt(ModelBuilder builder)
	{
		builder.Entity<FidoStoredCredential>().HasKey(m => m.Id);

		builder.Entity<ApplicationUser>(b =>
		{
			b.HasIndex(u => new { u.TenantId, u.Email });
			b.HasOne<Tenant>().WithMany().HasForeignKey(u => u.TenantId).OnDelete(DeleteBehavior.Restrict);

			// Global query filter: all Identity queries are scoped to the current tenant
			b.HasQueryFilter(u => _tenantContext == null
				|| _tenantContext.TenantId == Guid.Empty
				|| u.TenantId == _tenantContext.TenantId);
		});
	}
}
