using Aspire.Hosting;
using Dyvenix.App1.Common.Data.Config;
using Dyvenix.App1.Tests.Integration.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App1.Tests.Integration.Fixtures;

public class GlobalTestFixture : IAsyncLifetime
{
	public DistributedApplication App { get; private set; } = default!;
	public ServiceProvider Services { get; private set; } = default!;
	public IConfiguration Configuration { get; private set; } = default!;

	public async ValueTask InitializeAsync()
	{
		// --- Configuration ---
		Configuration = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json", optional: false)
			.AddEnvironmentVariables()
			.Build();

		// --- Aspire distributed app ---
		var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Aspire_AppHost>();
		appHost.Services.AddLogging(logging =>
		{
			logging.SetMinimumLevel(LogLevel.Debug);
			logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
			logging.AddFilter("Aspire.", LogLevel.Debug);
		});
		appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
		{
			clientBuilder.AddStandardResilienceHandler();
		});

		App = await appHost.BuildAsync();
		await App.StartAsync();

		var timeout = TimeSpan.FromSeconds(60);
		await App.ResourceNotifications
			.WaitForResourceHealthyAsync("portal-server")
			.WaitAsync(timeout);

		var services = new ServiceCollection();
		services.AddSingleton(Configuration);

		// --- Register test-specific services ---

		// App1Db
		var dataConfig = DataConfigBuilder.Build(Configuration);
		services.AddDataServices(dataConfig);

		services.AddSingleton<IDataManager, DataManager>();

		Services = services.BuildServiceProvider();
	}

	public async ValueTask DisposeAsync()
	{
		if (App != null)
			await App.DisposeAsync();
	}
}