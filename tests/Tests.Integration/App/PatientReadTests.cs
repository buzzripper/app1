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
	public async Task GetAllPaging_Success()
	{
		// Arrange

		var totalCount = _fixture.DataSet.PatientList.Count;
		if (totalCount < 6)
			throw new InvalidOperationException($"Test data should contain at least 6 patients for this test. Current count: {totalCount}");

		var request = new GetAllPagingReq();
		request.PageSize = 3;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;

		var lastPgOffset = totalCount / 3;
		if (totalCount % 3 == 0)
			lastPgOffset -= 1; // Adjust if total count is an exact multiple of page size
		var lastPgSize = totalCount - (lastPgOffset * 3);

		// Act
		request.PageOffset = 0;
		var firstPgList = await _patientApiClient.GetAllPaging(request);
		request.PageOffset = lastPgOffset;
		var lastPgList = await _patientApiClient.GetAllPaging(request);

		// Assert
		Assert.True(totalCount == firstPgList.TotalRowCount, $"First total count s/b {totalCount} but was {firstPgList.TotalRowCount}");
		Assert.True(request.PageSize == firstPgList.Items.Count, $"First page size s/b {request.PageSize} but was {firstPgList.Items.Count}");
		Assert.True(totalCount == lastPgList.TotalRowCount, $"Last total count s/b {totalCount} but was {firstPgList.TotalRowCount}");
		Assert.True(lastPgSize == lastPgList.Items.Count, $"Last page size s/b {lastPgSize} but was {lastPgList.Items.Count}");
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
