using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.App.Api.Entities;

namespace Dyvenix.App1.App.Api.Context;

public partial class App1Db : DbContext
{
	partial void OnModelCreatingExt(ModelBuilder builder);

	public App1Db(DbContextOptions<App1Db> options)
		: base(options)
	{
	}

	# region Properties

	public DbSet<Client> Client { get; set; }

	# endregion

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		this.OnModelCreatingExt(modelBuilder);

		#region Client

		modelBuilder.Entity<Client>(entity =>
		{
			entity.ToTable("Client");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.RowVersion).IsRowVersion();
			entity.Property(e => e.Key).IsRequired().HasMaxLength(50);
			entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
			entity.Property(e => e.BaseUrl).HasMaxLength(300);

			entity.HasIndex(e => e.Id, "IX_Client_Id").IsUnique();
			entity.HasIndex(e => e.Key, "IX_Client_Key").IsUnique();
			entity.HasIndex(e => e.Name, "IX_Client_Name").IsUnique();
		});

		#endregion

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
