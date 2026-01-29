using Dyvenix.App1.Common.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace Dyvenix.App1.Common.Data.Context;

public interface IDbContextFactory
{
	App1Db CreateDbContext();
}

public class DbContextFactory : IDbContextFactory
{
	protected readonly DataConfig _dataConfig;

	public DbContextFactory(DataConfig dataConfig)
	{
		_dataConfig = dataConfig;
	}

	public App1Db CreateDbContext()
	{
		var b = new DbContextOptionsBuilder<App1Db>();
		b.UseSqlServer(_dataConfig.ConnectionString);

		return new App1Db(b.Options);
	}
}