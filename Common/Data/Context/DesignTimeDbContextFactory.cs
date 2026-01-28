using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Dyvenix.App1.Data.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<App1Db>
{
	public App1Db CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<App1Db>();
		optionsBuilder.UseSqlServer("Password=?#cbz84A#znn5gL3pap;User ID=dyvenix_admin;Initial Catalog=dyvenix;Data Source=localhost;Encrypt=False;");

		return new App1Db(optionsBuilder.Options);
	}
}