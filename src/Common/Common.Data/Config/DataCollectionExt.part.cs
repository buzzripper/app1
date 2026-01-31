using Microsoft.Extensions.DependencyInjection;

namespace Dyvenix.App1.Common.Data.Config;

public static partial class DataCollectionExt
{
	static partial void AddDbServices(IServiceCollection services)
	{
		services.AddScoped<App1Db>();
	}
}
