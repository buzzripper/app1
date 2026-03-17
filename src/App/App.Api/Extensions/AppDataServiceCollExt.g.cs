using Dyvenix.App1.App.Api.Config;
using Dyvenix.App1.App.Api.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dyvenix.App1.App.Api.Extensions;

public static partial class AppApiServiceCollExt
{
	static partial void AddDataServices(IServiceCollection services, IConfiguration configuration)
	{
		var dataConfig = DataConfigBuilder.Build(configuration);
		services.AddSingleton(dataConfig);

		services.AddSingleton(sp =>
		{
			var b = new DbContextOptionsBuilder<App1Db>();
			b.UseSqlServer(dataConfig.ConnectionString);
			return b.Options;
		});
	}
}
