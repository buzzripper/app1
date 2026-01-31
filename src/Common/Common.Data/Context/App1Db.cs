//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 1/31/2026 2:55 PM. Any changes made to it will be lost.
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

	public DbSet<Person> Person { get; set; }
	public DbSet<Invoice> Invoice { get; set; }

	# endregion

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		#region Person

		modelBuilder.Entity<Person>(entity =>
		{
			entity.ToTable("Person");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
			entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
			entity.Property(e => e.Email).IsRequired().HasMaxLength(50);
			entity.Property(e => e.NewProperty).IsRequired().HasMaxLength(50);

			entity.HasIndex(e => e.Id, "IX_Person_Id").IsUnique();
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

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
