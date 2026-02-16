using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;
using Dyvenix.App1.Tests.Integration.Data;
using Dyvenix.App1.Tests.Integration.DataSets;
using Dyvenix.App1.Tests.Integration.Fixtures;

namespace Dyvenix.App1.Tests.Integration.Tests.App;

public sealed class PatientReadTestFixtureOld(GlobalTestFixture globalFixture) : IAsyncLifetime
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
public class PatientReadTestsOld : TestBase, IClassFixture<PatientReadTestFixture>
{
	private readonly PatientReadTestFixture _fixture;
	private IPatientService _patientService = default!;

	public PatientReadTestsOld(GlobalTestFixture globalFixture, PatientReadTestFixture fixture)
		: base(globalFixture)
	{
		_fixture = fixture;
	}

	public override async ValueTask InitializeAsync()
	{
		await base.InitializeAsync();
		_patientService = _scope.ServiceProvider.GetRequiredService<IPatientService>();
	}

	[Fact]
	public async Task GetById_Success()
	{
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var sample = _fixture.DataSet.PatientList.First();

		var result = await _patientService.GetById(sample.Id);
		Assert.Equal(sample.Id, result.Id);
	}

	[Fact]
	public async Task GetByEmail_Success()
	{
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var sample = _fixture.DataSet.PatientList.First();

		var result = await _patientService.GetByEmail(sample.Email);
		Assert.Equal(sample.Id, result.Id);
	}

	[Fact]
	public async Task GetByIdWithInvoices_Success()
	{
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var sample = _fixture.DataSet.PatientList.First();

		var result = await _patientService.GetByIdWithInvoices(sample.Id);
		Assert.Equal(sample.Id, result.Id);
	}

	[Fact]
	public async Task GetAllPaging_Success()
	{
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var sample = _fixture.DataSet.PatientList.First();
		var totalCount = _fixture.DataSet.PatientList.Count;
		if (totalCount < 6)
			throw new InvalidOperationException($"Test data should contain at least 6 items for this test. Current count: {totalCount}");
		var request = new GetAllPagingReq();
		request.PageSize = 3;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;

		var lastPgOffset = totalCount / request.PageSize;
		if (totalCount % request.PageSize == 0)
			lastPgOffset -= 1;
		var lastPgSize = totalCount - (lastPgOffset * request.PageSize);

		request.PageOffset = 0;
		var firstPgList = await _patientService.GetAllPaging(request);
		request.PageOffset = lastPgOffset;
		var lastPgList = await _patientService.GetAllPaging(request);

		Assert.True(totalCount == firstPgList.TotalRowCount, $"First total count s/b {totalCount} but was {firstPgList.TotalRowCount}");
		Assert.True(request.PageSize == firstPgList.Items.Count, $"First page size s/b {request.PageSize} but was {firstPgList.Items.Count}");
		Assert.True(totalCount == lastPgList.TotalRowCount, $"Last total count s/b {totalCount} but was {lastPgList.TotalRowCount}");
		Assert.True(lastPgSize == lastPgList.Items.Count, $"Last page size s/b {lastPgSize} but was {lastPgList.Items.Count}");
	}

	[Fact]
	public async Task SearchByLastNamePaging_Success()
	{
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var sample = _fixture.DataSet.PatientList.First();
		var totalCount = _fixture.DataSet.PatientList.Count;
		if (totalCount < 6)
			throw new InvalidOperationException($"Test data should contain at least 6 items for this test. Current count: {totalCount}");
		var request = new SearchByLastNamePagingReq();
		request.PageSize = 3;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		request.LastName = sample.LastName;

		var lastPgOffset = totalCount / request.PageSize;
		if (totalCount % request.PageSize == 0)
			lastPgOffset -= 1;
		var lastPgSize = totalCount - (lastPgOffset * request.PageSize);

		request.PageOffset = 0;
		var firstPgList = await _patientService.SearchByLastNamePaging(request);
		request.PageOffset = lastPgOffset;
		var lastPgList = await _patientService.SearchByLastNamePaging(request);

		Assert.True(totalCount == firstPgList.TotalRowCount, $"First total count s/b {totalCount} but was {firstPgList.TotalRowCount}");
		Assert.True(request.PageSize == firstPgList.Items.Count, $"First page size s/b {request.PageSize} but was {firstPgList.Items.Count}");
		Assert.True(totalCount == lastPgList.TotalRowCount, $"Last total count s/b {totalCount} but was {lastPgList.TotalRowCount}");
		Assert.True(lastPgSize == lastPgList.Items.Count, $"Last page size s/b {lastPgSize} but was {lastPgList.Items.Count}");
	}

	[Fact]
	public async Task SearchByLastNameSorting_Success()
	{
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var sample = _fixture.DataSet.PatientList.First();
		var request = new SearchByLastNameSortingReq();
		request.SortBy = "Id";
		request.SortDesc = false;
		request.LastName = sample.LastName;

		var results = await _patientService.SearchByLastNameSorting(request);
		Assert.Contains(results, item => item.Id == sample.Id);
	}

	[Fact]
	public async Task SearchByLastNamePagingSorting_Success()
	{
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var sample = _fixture.DataSet.PatientList.First();
		var totalCount = _fixture.DataSet.PatientList.Count;
		if (totalCount < 6)
			throw new InvalidOperationException($"Test data should contain at least 6 items for this test. Current count: {totalCount}");
		var request = new SearchByLastNamePagingSortingReq();
		request.PageSize = 3;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		request.SortBy = "Id";
		request.SortDesc = false;
		request.LastName = sample.LastName;

		var lastPgOffset = totalCount / request.PageSize;
		if (totalCount % request.PageSize == 0)
			lastPgOffset -= 1;
		var lastPgSize = totalCount - (lastPgOffset * request.PageSize);

		request.PageOffset = 0;
		var firstPgList = await _patientService.SearchByLastNamePagingSorting(request);
		request.PageOffset = lastPgOffset;
		var lastPgList = await _patientService.SearchByLastNamePagingSorting(request);

		Assert.True(totalCount == firstPgList.TotalRowCount, $"First total count s/b {totalCount} but was {firstPgList.TotalRowCount}");
		Assert.True(request.PageSize == firstPgList.Items.Count, $"First page size s/b {request.PageSize} but was {firstPgList.Items.Count}");
		Assert.True(totalCount == lastPgList.TotalRowCount, $"Last total count s/b {totalCount} but was {lastPgList.TotalRowCount}");
		Assert.True(lastPgSize == lastPgList.Items.Count, $"Last page size s/b {lastPgSize} but was {lastPgList.Items.Count}");
	}

	[Fact]
	public async Task GetAllSorting_Success()
	{
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var sample = _fixture.DataSet.PatientList.First();
		var request = new GetAllSortingReq();
		request.SortBy = "Id";
		request.SortDesc = false;

		var results = await _patientService.GetAllSorting(request);
		Assert.Contains(results, item => item.Id == sample.Id);
	}

	[Fact]
	public async Task SearchByLastEmailOpt_Success()
	{
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var sample = _fixture.DataSet.PatientList.First();
		var request = new SearchByLastEmailOptReq();
		request.LastName = sample.LastName;
		request.Email = sample.Email;

		var results = await _patientService.SearchByLastEmailOpt(request);
		Assert.Contains(results, item => item.Id == sample.Id);
	}

	[Fact]
	public async Task SearchByEmail_Success()
	{
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var sample = _fixture.DataSet.PatientList.First();
		var request = new SearchByEmailReq();
		request.Email = sample.Email;

		var results = await _patientService.SearchByEmail(request);
		Assert.Contains(results, item => item.Id == sample.Id);
	}

	[Fact]
	public async Task GetActive_Success()
	{
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var sample = _fixture.DataSet.PatientList.First();

		var results = await _patientService.GetActive();
		Assert.Contains(results, item => item.Id == sample.Id);
	}

	[Fact]
	public async Task GetAllPagingSorting_Success()
	{
		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");
		var sample = _fixture.DataSet.PatientList.First();
		var totalCount = _fixture.DataSet.PatientList.Count;
		if (totalCount < 6)
			throw new InvalidOperationException($"Test data should contain at least 6 items for this test. Current count: {totalCount}");
		var request = new GetAllPagingSortingReq();
		request.PageSize = 3;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		request.SortBy = "Id";
		request.SortDesc = false;

		var lastPgOffset = totalCount / request.PageSize;
		if (totalCount % request.PageSize == 0)
			lastPgOffset -= 1;
		var lastPgSize = totalCount - (lastPgOffset * request.PageSize);

		request.PageOffset = 0;
		var firstPgList = await _patientService.GetAllPagingSorting(request);
		request.PageOffset = lastPgOffset;
		var lastPgList = await _patientService.GetAllPagingSorting(request);

		Assert.True(totalCount == firstPgList.TotalRowCount, $"First total count s/b {totalCount} but was {firstPgList.TotalRowCount}");
		Assert.True(request.PageSize == firstPgList.Items.Count, $"First page size s/b {request.PageSize} but was {firstPgList.Items.Count}");
		Assert.True(totalCount == lastPgList.TotalRowCount, $"Last total count s/b {totalCount} but was {lastPgList.TotalRowCount}");
		Assert.True(lastPgSize == lastPgList.Items.Count, $"Last page size s/b {lastPgSize} but was {lastPgList.Items.Count}");
	}

	[Fact]
	public async Task SearchActiveLastName_Success()
	{
		var sample = _fixture.DataSet.PatientList.FirstOrDefault(p => p.IsActive);
		if (sample == null)
			throw new InvalidOperationException("No active patients found.");


		var results = await _patientService.SearchActiveLastName(sample.LastName);
		Assert.Contains(results, item => item.Id == sample.Id);
	}
}
