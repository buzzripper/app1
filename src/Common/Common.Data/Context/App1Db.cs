//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/2/2026 8:28 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.Common.Data.Shared.Entities;

namespace Dyvenix.App1.Common.Data;

public partial class App1Db : DbContext
{
	public App1Db(DbContextOptions<App1Db> options)
		: base(options)
	{
	}

	# region Properties

	public DbSet<Patient> Patient { get; set; }
	public DbSet<Invoice> Invoice { get; set; }
	public DbSet<AppUser> AppUser { get; set; }

	# endregion

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		#region Patient

		modelBuilder.Entity<Patient>(entity =>
		{
			entity.ToTable("Patient");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
			entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
			entity.Property(e => e.Email).HasMaxLength(50);

			entity.HasIndex(e => e.Id, "IX_Patient_Id").IsUnique();
		});

		#endregion

		#region Invoice

		modelBuilder.Entity<Invoice>(entity =>
		{
			entity.ToTable("Invoice");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.PersonId).IsRequired();
			entity.Property(e => e.Amount).IsRequired();

			entity.HasIndex(e => e.Id, "IX_Invoice_Id").IsUnique();
			entity.HasIndex(e => e.PersonId, "IX_Invoice_PersonId");
			entity.HasIndex(e => e.Amount, "IX_Invoice_Amount");
		});

		#endregion

		#region AppUser

		modelBuilder.Entity<AppUser>(entity =>
		{
			entity.ToTable("AppUser");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Username).IsRequired().HasMaxLength(50);

			entity.HasIndex(e => e.Id, "IX_AppUser_Id").IsUnique();
		});

		#endregion

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
