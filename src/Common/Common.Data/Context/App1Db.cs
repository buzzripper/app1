//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 1/28/2026 3:17 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Data.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dyvenix.App1.Common.Data.Context;

public partial class App1Db : DbContext
{
	public App1Db(DbContextOptions<App1Db> options)
		: base(options)
	{
	}

	#region Properties

	public DbSet<Person> Person { get; set; }

	#endregion

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

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
