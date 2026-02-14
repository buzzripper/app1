using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;
using Dyvenix.App1.Tests.Integration.Data;
using Dyvenix.App1.Tests.Integration.DataSets;
using Dyvenix.App1.Tests.Integration.Fixtures;

namespace Dyvenix.App1.Tests.Integration.App;

public sealed class PatientReadTestFixture(GlobalTestFixture globalFixture) : IAsyncLifetime
{
	public GlobalTestFixture GlobalFixture { get; } = globalFixture;
	public TestDataSet DataSet { get; private set; } = default!;

	public async ValueTask InitializeAsync()
	{
		var dataManager = GlobalFixture.Services.GetRequiredService<IDataManager>();
		DataSet = await dataManager.Reset(DataSetType.Main.ToString());
	}

	public ValueTask DisposeAsync() => default;
}

[Collection(nameof(GlobalTestCollection))]
public class PatientReadTests : TestBase, IClassFixture<PatientReadTestFixture>
{
	private readonly PatientReadTestFixture _fixture;
	private IPatientService _patientApiClient = default!;

	public PatientReadTests(GlobalTestFixture globalFixture, PatientReadTestFixture fixture)
		: base(globalFixture)
	{
		_fixture = fixture;
	}

	public override async ValueTask InitializeAsync()
	{
		await base.InitializeAsync();
		_patientApiClient = _scope.ServiceProvider.GetRequiredService<IPatientService>();
	}

	[Fact]
	public async Task Ping()
	{
		// Arrange
		using var httpClient = _globalFixture.App.CreateHttpClient("portal-server");

		// Act
		using var response = await httpClient.GetAsync("/api/app/system/ping", TestContext.Current.CancellationToken);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	[Fact]
	public async Task GetCount2()
	{
		// Arrange
		var pgReq = new GetAllPagingReq
		{
			PageOffset = 0,
			PageSize = 10
		};

		// Act
		using var httpClient = _globalFixture.App.CreateHttpClient("portal-server");
		using var response = await httpClient.GetAsync("/api/app/system/ping", TestContext.Current.CancellationToken);
		var pgList = await _patientApiClient.GetAllPaging(pgReq);

		// Assert
		Assert.True(pgList.Items.Count > 0, "Expected at least one patient in the paged list.");
	}

	[Fact]
	public async Task CheckPatientCount_Success()
	{
		// Arrange
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var testDataPatientCount = _fixture.DataSet.PatientList.Count;

		// Act
		var dbUserCount = _db.Patient.ToList().Count();

		// Assert
		TestContext.Current.TestOutputHelper?.WriteLine($"Patient count in test data: {testDataPatientCount}");
		TestContext.Current.TestOutputHelper?.WriteLine($"Patient count in db: {dbUserCount}");

		Assert.Equal(testDataPatientCount, dbUserCount);

	}

	[Fact]
	public async Task GetById_Success()
	{
		// Arrange
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var patientId = _fixture.DataSet.PatientList.First().Id;

		// Act
		var patientSvcClient = _fixture.GlobalFixture.Services.GetRequiredService<IPatientService>();
		var patient = await patientSvcClient.GetById(patientId);

		// Assert
		Assert.Equal(patientId, patient.Id);

	}
}
