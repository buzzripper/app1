using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Dyvenix.App1.Common.Data.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<App1Db>
{
	public App1Db CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<App1Db>();
		optionsBuilder.UseSqlServer("Server=.;Database=App1;Trusted_Connection=True;TrustServerCertificate=True");

		return new App1Db(optionsBuilder.Options);
	}
}