using Dyvenix.App1.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dyvenix.App1.Data.Config;

public static class DataSvcCollExt
{
	public static void AddDyvenixDataServices(this IServiceCollection services, DataConfig dataConfig)
	{
		services.AddSingleton(dataConfig);

		services.AddSingleton(sp =>
		{
			var b = new DbContextOptionsBuilder<App1Db>();
			b.UseSqlServer(dataConfig.ConnectionString);
			return b.Options;
		});

		services.AddSingleton<IDbContextFactory, DbContextFactory>();
	}
}