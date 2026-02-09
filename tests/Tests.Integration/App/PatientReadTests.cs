using Dyvenix.App1.Tests.Integration.Data;
using Dyvenix.App1.Tests.Integration.DataSets;
using Dyvenix.App1.Tests.Integration.Fixtures;

namespace Dyvenix.App1.Tests.Integration.App;

public sealed class PatientReadTestFixture(GlobalTestFixture globalFixture) : IAsyncLifetime
{
	public GlobalTestFixture GlobalFixture { get; } = globalFixture;
	public IDataSet DataSet { get; private set; } = default!;

	public async ValueTask InitializeAsync()
	{
		var dataManager = GlobalFixture.Services.GetRequiredService<IDataManager>();
		DataSet = await dataManager.Reset(DataSetType.Default);
	}

	public ValueTask DisposeAsync() => default;
}

[Collection(nameof(GlobalTestCollection))]
public class PatientReadTests : TestBase, IClassFixture<PatientReadTestFixture>
{
	private readonly PatientReadTestFixture _fixture;

	public PatientReadTests(GlobalTestFixture globalFixture, PatientReadTestFixture fixture)
		: base(globalFixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public async Task Ping()
	{
		// Arrange
		using var httpClient = _globalFixture.App.CreateHttpClient("app-server");

		// Act
		using var response = await httpClient.GetAsync("/api/app/v1/system/ping", TestContext.Current.CancellationToken);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	[Fact]
	public async Task GetPatientCount_Success()
	{
		// Arrange
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var userCount = _db.Patient.ToList().Count();
		TestContext.Current.TestOutputHelper?.WriteLine($"Patient count in db: {userCount}");
		using var httpClient = _globalFixture.App.CreateHttpClient("app-server");

		// Act
		using var response = await httpClient.GetAsync("/api/app/v1/system/ping", TestContext.Current.CancellationToken);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}
}
