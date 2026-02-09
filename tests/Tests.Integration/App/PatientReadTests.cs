using Dyvenix.App1.Tests.Integration.Data;
using Dyvenix.App1.Tests.Integration.Fixtures;

namespace Dyvenix.App1.Tests.Integration.App;

public sealed class PatientReadTestFixture
{
	public GlobalTestFixture GlobalFixture { get; private set; } = default!;
	public bool IsInitialized { get; private set; }

	public async Task InitializeAsync(GlobalTestFixture globalFixture)
	{
		if (IsInitialized)
			return;

		GlobalFixture = globalFixture;

		// One-time per-class setup that needs GlobalTestFixture
		// e.g., seed Patient data using GlobalFixture.Services
		var dataManager = globalFixture.Services.GetRequiredService<IDataManager>();
		// await dataManager.SeedAsync(...);

		IsInitialized = true;
	}
}

[Collection(nameof(GlobalTestCollection))]
public class PatientReadTests(GlobalTestFixture fixture, PatientReadTestFixture classFixture) : TestBase(fixture), IClassFixture<PatientReadTestFixture>
{
	public override async ValueTask InitializeAsync()
	{
		await base.InitializeAsync();

		// Pass the global fixture to the class fixture (runs once due to IsInitialized guard)
		await classFixture.InitializeAsync(_fixture);
	}

	[Fact]
	public async Task Ping()
	{
		// Arrange
		using var httpClient = _fixture.App.CreateHttpClient("app-server");

		// Act
		using var response = await httpClient.GetAsync("/api/app/v1/system/ping", TestContext.Current.CancellationToken);

		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");

		var userCount = _db.Patient.ToList().Count();
		TestContext.Current.TestOutputHelper?.WriteLine($"Patient count: {userCount}");

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}
}
