//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/28/2026 11:36 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Auth.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dyvenix.App1.Auth.Data.Context;

public partial class AuthDbContext : IdentityDbContext<ApplicationUser>
{
	partial void OnModelCreatingExt(ModelBuilder builder);

	public AuthDbContext(DbContextOptions<AuthDbContext> options)
		: base(options)
	{
	}

	#region Properties

	public DbSet<Tenant> Tenant { get; set; }

	#endregion

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		this.OnModelCreatingExt(modelBuilder);

		#region Tenant

		modelBuilder.Entity<Tenant>(entity =>
		{
			entity.ToTable("Tenant");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
			entity.Property(e => e.Slug).IsRequired().HasMaxLength(100);
			entity.Property(e => e.AuthMode).IsRequired();
			entity.Property(e => e.ExternalAuthority).HasMaxLength(500);
			entity.Property(e => e.ExternalClientId).HasMaxLength(200);
			entity.Property(e => e.ExternalClientSecret).HasMaxLength(500);
			entity.Property(e => e.ADDcHost).HasMaxLength(200);
			entity.Property(e => e.ADDomain).HasMaxLength(200);
			entity.Property(e => e.ADLdapPort);
			entity.Property(e => e.ADBaseDn).HasMaxLength(200);
			entity.Property(e => e.IsActive).IsRequired();
			entity.Property(e => e.CreatedAt).IsRequired();

			entity.HasIndex(e => e.Id, "IX_Tenant_Id");
		});

		#endregion

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
