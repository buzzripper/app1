//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 1/25/2026 8:35 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.Data.Shared.Entities;

namespace Dyvenix.App1.Data.Context;

public partial class App1Db : DbContext
{
	public App1Db(DbContextOptions<App1Db> options)
		: base(options)
	{
	}

	# region Properties

	public DbSet<Company> Company { get; set; }

	# endregion

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		#region Company

		modelBuilder.Entity<Company>(entity =>
		{
			entity.ToTable("Company");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Name).IsRequired().HasMaxLength(50);

			entity.HasIndex(e => e.Id, "IX_Company_Id").IsUnique();
		});

		#endregion

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
