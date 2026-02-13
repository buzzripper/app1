using Aspire.Hosting;
using Dyvenix.App1.App.Shared.Extensions;
using Dyvenix.App1.Auth.Shared.Extensions;
using Dyvenix.App1.Common.Data;
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

		// App1Db
		var dataConfig = DataConfigBuilder.Build(Configuration);
		services.AddDataServices(dataConfig);
		services.AddSingleton<IDataManager>(sp =>
		{
			var db = sp.GetRequiredService<App1Db>();
			return new DataManager(db);
		});

		// App module
		services.AddAppSharedServices(Configuration, false);

		// Auth module
		services.AddAuthSharedServices(Configuration, false);

		// Initialize test data
		var dataManager = Services.GetRequiredService<IDataManager>();
		await dataManager.Initialize();

		Services = services.BuildServiceProvider();
	}

	public async ValueTask DisposeAsync()
	{
		if (App != null)
			await App.DisposeAsync();
	}
}