//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/14/2026 5:02 PM. Any changes made to it will be lost.
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
	public DbSet<Practice> Practice { get; set; }
	public DbSet<Category> Category { get; set; }

	# endregion

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		#region Patient

		modelBuilder.Entity<Patient>(entity =>
		{
			entity.ToTable("Patient");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.RowVersion).IsRowVersion();
			entity.Property(e => e.PracticeId).IsRequired();
			entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
			entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
			entity.Property(e => e.Email).HasMaxLength(50);
			entity.Property(e => e.IsActive).IsRequired();

			entity.HasIndex(e => e.Id, "IX_Patient_Id").IsUnique();
			entity.HasIndex(e => e.PracticeId, "IX_Patient_PracticeId");
		});

		#endregion

		#region Invoice

		modelBuilder.Entity<Invoice>(entity =>
		{
			entity.ToTable("Invoice");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.PatientId).IsRequired();
			entity.Property(e => e.CategoryId).IsRequired();
			entity.Property(e => e.Amount).IsRequired();
			entity.Property(e => e.Memo).IsRequired().HasMaxLength(200);

			entity.HasIndex(e => e.Id, "IX_Invoice_Id").IsUnique();
			entity.HasIndex(e => e.PatientId, "IX_Invoice_PatientId");
			entity.HasIndex(e => e.Amount, "IX_Invoice_Amount");
			entity.HasIndex(e => e.CategoryId, "IX_Invoice_CategoryId");
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

		#region Practice

		modelBuilder.Entity<Practice>(entity =>
		{
			entity.ToTable("Practice");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Name).IsRequired().HasMaxLength(50);

			entity.HasIndex(e => e.Id, "IX_Practice_Id").IsUnique();
		});

		#endregion

		#region Category

		modelBuilder.Entity<Category>(entity =>
		{
			entity.ToTable("Category");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Name).IsRequired().HasMaxLength(50);

			entity.HasIndex(e => e.Id, "IX_Category_Id").IsUnique();
		});

		#endregion

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
