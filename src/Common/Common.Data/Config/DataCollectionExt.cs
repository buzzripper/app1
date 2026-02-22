using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dyvenix.App1.Common.Data.Config;

public static partial class DataCollectionExt
{
	// Declaration of partial method for code-generated services
	static partial void AddDbServices(IServiceCollection services);

	public static void AddDataServices(this IServiceCollection services, DataConfig dataConfig)
	{
		services.AddSingleton(dataConfig);

		services.AddSingleton(sp =>
		{
			var b = new DbContextOptionsBuilder<App1Db>();
			b.UseSqlServer(dataConfig.ConnectionString);
			return b.Options;
		});

		// Add code-generated services
		AddDbServices(services);
	}
}