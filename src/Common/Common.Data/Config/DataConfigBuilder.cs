using Microsoft.Extensions.Configuration;

namespace Dyvenix.App1.Common.Data.Config;

public static class DataConfigBuilder
{
	private const string cConfigSectionName = "DataConfig";

	public static DataConfig Build(IConfiguration configuration)
	{
		var dataConfig = configuration.GetSection(cConfigSectionName).Get<DataConfig>();
		if (dataConfig == null)
			throw new ApplicationException($"Unable to retrieve {cConfigSectionName} section from appsettings.json file.");

		return dataConfig;
	}
}
