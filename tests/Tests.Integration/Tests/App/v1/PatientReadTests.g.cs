//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/18/2026 7:27 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Linq;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.Tests.Integration.Data;
using Dyvenix.App1.Tests.Integration.DataSets;
using Dyvenix.App1.Tests.Integration.Fixtures;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;

namespace Dyvenix.App1.Tests.Integration.Tests.App.v1;

public class PatientReadTestFixture(GlobalTestFixture globalFixture) : IAsyncLifetime
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
	private IPatientService _patientService = default!;

	public PatientReadTests(GlobalTestFixture globalFixture, PatientReadTestFixture fixture)
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
		// Arrange
		var patientSample = _db.Patient.First();
		var id = patientSample.Id;

		// Act
		var result = await _patientService.GetById(id);

		// Assert
		Assert.Equal(patientSample.Id, result.Id);
	}

	[Fact]
	public async Task GetById_NotFound()
	{
		// Arrange
		var patientSample = _db.Patient.First();
		var id = patientSample.Id;
		var invalidId = Guid.NewGuid();

		// Act
		var result = await _patientService.GetById(invalidId);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetByEmail_Success()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.Email));
		var email = patientSample.Email;

		// Act
		var result = await _patientService.GetByEmail(email);

		// Assert
		Assert.Equal(patientSample.Id, result.Id);
	}

	[Fact]
	public async Task GetByEmail_NotFound()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.Email));
		var email = patientSample.Email;
		var invalidEmail = "__invalid__";

		// Act
		var result = await _patientService.GetByEmail(invalidEmail);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetAllPaging_Success()
	{
		// Arrange
		var patientSample = _db.Patient.First();
		var request = new GetAllPagingReq();
		request.PageSize = 0;
		request.PageOffset = 0;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		var expectedList = _db.Patient.ToList();

		// Act
		var result = await _patientService.GetAllPaging(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Items.Count);
		Assert.Equal(expectedList.Count, result.TotalRowCount);
	}

	[Fact]
	public async Task GetAllPaging_PagingSuccess()
	{
		// Arrange
		var patientSample = _db.Patient.First();
		var request = new GetAllPagingReq();
		request.PageSize = 3;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		var expectedList = _db.Patient.ToList();
		var totalCount = expectedList.Count;
		var lastPgOffset = totalCount == 0 ? 0 : totalCount / request.PageSize;
		if (totalCount % request.PageSize == 0 && totalCount > 0)
			lastPgOffset -= 1;
		var lastPgSize = totalCount == 0 ? 0 : totalCount - (lastPgOffset * request.PageSize);

		// Act
		request.PageOffset = 0;
		var firstPgList = await _patientService.GetAllPaging(request);
		request.PageOffset = lastPgOffset;
		var lastPgList = await _patientService.GetAllPaging(request);

		// Assert
		Assert.True(totalCount == firstPgList.TotalRowCount, $"First total count s/b {totalCount} but was {firstPgList.TotalRowCount}");
		Assert.True(request.PageSize == firstPgList.Items.Count, $"First page size s/b {request.PageSize} but was {firstPgList.Items.Count}");
		Assert.True(totalCount == lastPgList.TotalRowCount, $"Last total count s/b {totalCount} but was {lastPgList.TotalRowCount}");
		Assert.True(lastPgSize == lastPgList.Items.Count, $"Last page size s/b {lastPgSize} but was {lastPgList.Items.Count}");
	}

	[Fact]
	public async Task SearchByLastNamePaging_Success()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName));
		var lastName = patientSample.LastName;
		var request = new SearchByLastNamePagingReq();
		request.PageSize = 0;
		request.PageOffset = 0;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		request.LastName = lastName;
		var expectedList = _db.Patient.Where(x => x.LastName != null && x.LastName.Contains(lastName)).ToList();

		// Act
		var result = await _patientService.SearchByLastNamePaging(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Items.Count);
		Assert.Equal(expectedList.Count, result.TotalRowCount);
	}

	[Fact]
	public async Task SearchByLastNamePaging_NoResults()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName));
		var lastName = patientSample.LastName;
		var invalidLastName = "__invalid__";
		var request = new SearchByLastNamePagingReq();
		request.PageSize = 0;
		request.PageOffset = 0;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		request.LastName = invalidLastName;

		// Act
		var result = await _patientService.SearchByLastNamePaging(request);

		// Assert
		Assert.Empty(result.Items);
	}

	[Fact]
	public async Task SearchByLastNamePaging_PagingSuccess()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName));
		var lastName = patientSample.LastName.Substring(0, 1);
		var request = new SearchByLastNamePagingReq();
		request.LastName = lastName;
		request.PageSize = 3;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		var expectedList = _db.Patient.Where(x => x.LastName != null && x.LastName.Contains(lastName)).ToList();
		var totalCount = expectedList.Count;
		var lastPgOffset = totalCount == 0 ? 0 : totalCount / request.PageSize;
		if (totalCount % request.PageSize == 0 && totalCount > 0)
			lastPgOffset -= 1;
		var lastPgSize = totalCount == 0 ? 0 : totalCount - (lastPgOffset * request.PageSize);

		// Act
		request.PageOffset = 0;
		var firstPgList = await _patientService.SearchByLastNamePaging(request);
		request.PageOffset = lastPgOffset;
		var lastPgList = await _patientService.SearchByLastNamePaging(request);

		// Assert
		Assert.True(totalCount == firstPgList.TotalRowCount, $"First total count s/b {totalCount} but was {firstPgList.TotalRowCount}");
		Assert.True(request.PageSize == firstPgList.Items.Count, $"First page size s/b {request.PageSize} but was {firstPgList.Items.Count}");
		Assert.True(totalCount == lastPgList.TotalRowCount, $"Last total count s/b {totalCount} but was {lastPgList.TotalRowCount}");
		Assert.True(lastPgSize == lastPgList.Items.Count, $"Last page size s/b {lastPgSize} but was {lastPgList.Items.Count}");
	}

	[Fact]
	public async Task SearchByLastNameSorting_Success()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName));
		var lastName = patientSample.LastName;
		var request = new SearchByLastNameSortingReq();
		request.LastName = lastName;
		var expectedList = _db.Patient.Where(x => x.LastName != null && x.LastName.Contains(lastName)).ToList();

		// Act
		var result = await _patientService.SearchByLastNameSorting(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}

	[Fact]
	public async Task SearchByLastNameSorting_NoResults()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName));
		var lastName = patientSample.LastName;
		var invalidLastName = "__invalid__";
		var request = new SearchByLastNameSortingReq();
		request.LastName = invalidLastName;

		// Act
		var result = await _patientService.SearchByLastNameSorting(request);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public async Task SearchByLastNameSorting_SortingSuccess()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName));
		var lastName = patientSample.LastName;
		var request = new SearchByLastNameSortingReq();
		request.LastName = lastName;
		request.SortBy = Patient.PropNames.FirstName;
		var expectedAsc = _db.Patient.Where(x => x.LastName != null && x.LastName.Contains(lastName)).ToList().OrderBy(x => x.FirstName).ToList();
		var expectedDesc = _db.Patient.Where(x => x.LastName != null && x.LastName.Contains(lastName)).ToList().OrderByDescending(x => x.FirstName).ToList();

		// Act
		request.SortDesc = false;
		var ascResult = await _patientService.SearchByLastNameSorting(request);
		request.SortDesc = true;
		var descResult = await _patientService.SearchByLastNameSorting(request);

		// Assert
		Assert.Equal(expectedAsc.Select(x => x.Id), ascResult.Select(x => x.Id));
		Assert.Equal(expectedDesc.Select(x => x.Id), descResult.Select(x => x.Id));
	}

	[Fact]
	public async Task SearchByLastNamePagingSorting_Success()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName));
		var lastName = patientSample.LastName;
		var request = new SearchByLastNamePagingSortingReq();
		request.PageSize = 0;
		request.PageOffset = 0;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		request.LastName = lastName;
		var expectedList = _db.Patient.Where(x => x.LastName != null && x.LastName.Contains(lastName)).ToList();

		// Act
		var result = await _patientService.SearchByLastNamePagingSorting(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Items.Count);
		Assert.Equal(expectedList.Count, result.TotalRowCount);
	}

	[Fact]
	public async Task SearchByLastNamePagingSorting_NoResults()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName));
		var lastName = patientSample.LastName;
		var invalidLastName = "__invalid__";
		var request = new SearchByLastNamePagingSortingReq();
		request.PageSize = 0;
		request.PageOffset = 0;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		request.LastName = invalidLastName;

		// Act
		var result = await _patientService.SearchByLastNamePagingSorting(request);

		// Assert
		Assert.Empty(result.Items);
	}

	[Fact]
	public async Task SearchByLastNamePagingSorting_PagingSuccess()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName));
		var lastName = patientSample.LastName.Substring(0, 1);
		var request = new SearchByLastNamePagingSortingReq();
		request.LastName = lastName;
		request.PageSize = 3;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		var expectedList = _db.Patient.Where(x => x.LastName != null && x.LastName.Contains(lastName)).ToList();
		var totalCount = expectedList.Count;
		var lastPgOffset = totalCount == 0 ? 0 : totalCount / request.PageSize;
		if (totalCount % request.PageSize == 0 && totalCount > 0)
			lastPgOffset -= 1;
		var lastPgSize = totalCount == 0 ? 0 : totalCount - (lastPgOffset * request.PageSize);

		// Act
		request.PageOffset = 0;
		var firstPgList = await _patientService.SearchByLastNamePagingSorting(request);
		request.PageOffset = lastPgOffset;
		var lastPgList = await _patientService.SearchByLastNamePagingSorting(request);

		// Assert
		Assert.True(totalCount == firstPgList.TotalRowCount, $"First total count s/b {totalCount} but was {firstPgList.TotalRowCount}");
		Assert.True(request.PageSize == firstPgList.Items.Count, $"First page size s/b {request.PageSize} but was {firstPgList.Items.Count}");
		Assert.True(totalCount == lastPgList.TotalRowCount, $"Last total count s/b {totalCount} but was {lastPgList.TotalRowCount}");
		Assert.True(lastPgSize == lastPgList.Items.Count, $"Last page size s/b {lastPgSize} but was {lastPgList.Items.Count}");
	}

	[Fact]
	public async Task SearchByLastNamePagingSorting_SortingSuccess()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName));
		var lastName = patientSample.LastName;
		var request = new SearchByLastNamePagingSortingReq();
		request.PageSize = 0;
		request.PageOffset = 0;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		request.LastName = lastName;
		request.SortBy = Patient.PropNames.FirstName;
		var expectedAsc = _db.Patient.Where(x => x.LastName != null && x.LastName.Contains(lastName)).ToList().OrderBy(x => x.FirstName).ToList();
		var expectedDesc = _db.Patient.Where(x => x.LastName != null && x.LastName.Contains(lastName)).ToList().OrderByDescending(x => x.FirstName).ToList();

		// Act
		request.SortDesc = false;
		var ascResult = await _patientService.SearchByLastNamePagingSorting(request);
		request.SortDesc = true;
		var descResult = await _patientService.SearchByLastNamePagingSorting(request);

		// Assert
		Assert.Equal(expectedAsc.Select(x => x.Id), ascResult.Items.Select(x => x.Id));
		Assert.Equal(expectedDesc.Select(x => x.Id), descResult.Items.Select(x => x.Id));
	}

	[Fact]
	public async Task GetByIdWithInvoices_Success()
	{
		// Arrange
		var patientSample = _db.Patient.First();
		var id = patientSample.Id;

		// Act
		var result = await _patientService.GetByIdWithInvoices(id);

		// Assert
		Assert.Equal(patientSample.Id, result.Id);
	}

	[Fact]
	public async Task GetByIdWithInvoices_NotFound()
	{
		// Arrange
		var patientSample = _db.Patient.First();
		var id = patientSample.Id;
		var invalidId = Guid.NewGuid();

		// Act
		var result = await _patientService.GetByIdWithInvoices(invalidId);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetAllSorting_Success()
	{
		// Arrange
		var patientSample = _db.Patient.First();
		var request = new GetAllSortingReq();
		var expectedList = _db.Patient.ToList();

		// Act
		var result = await _patientService.GetAllSorting(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}

	[Fact]
	public async Task GetAllSorting_SortingSuccess()
	{
		// Arrange
		var patientSample = _db.Patient.First();
		var request = new GetAllSortingReq();
		request.SortBy = Patient.PropNames.FirstName;
		var expectedAsc = _db.Patient.ToList().OrderBy(x => x.FirstName).ToList();
		var expectedDesc = _db.Patient.ToList().OrderByDescending(x => x.FirstName).ToList();

		// Act
		request.SortDesc = false;
		var ascResult = await _patientService.GetAllSorting(request);
		request.SortDesc = true;
		var descResult = await _patientService.GetAllSorting(request);

		// Assert
		Assert.Equal(expectedAsc.Select(x => x.Id), ascResult.Select(x => x.Id));
		Assert.Equal(expectedDesc.Select(x => x.Id), descResult.Select(x => x.Id));
	}

	[Fact]
	public async Task SearchByLastEmailOpt_Success()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName) && !string.IsNullOrWhiteSpace(x.Email));
		var lastName = patientSample.LastName;
		var email = patientSample.Email;
		var request = new SearchByLastEmailOptReq();
		request.LastName = lastName;
		request.Email = email;
		var expectedList = _db.Patient.Where(x => x.LastName != null && x.LastName.Contains(lastName) && x.Email != null && x.Email.Contains(email)).ToList();

		// Act
		var result = await _patientService.SearchByLastEmailOpt(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}

	[Fact]
	public async Task SearchByLastEmailOpt_OptionalFiltersOmitted_Success()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName));
		var lastName = patientSample.LastName;
		var request = new SearchByLastEmailOptReq();
		request.LastName = lastName;
		var expectedList = _db.Patient.Where(x => x.LastName != null && x.LastName.Contains(lastName)).ToList();

		// Act
		var result = await _patientService.SearchByLastEmailOpt(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}

	[Fact]
	public async Task SearchByLastEmailOpt_NoResults()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName) && !string.IsNullOrWhiteSpace(x.Email));
		var lastName = patientSample.LastName;
		var email = patientSample.Email;
		var invalidLastName = "__invalid__";
		var request = new SearchByLastEmailOptReq();
		request.LastName = invalidLastName;
		request.Email = email;

		// Act
		var result = await _patientService.SearchByLastEmailOpt(request);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public async Task SearchByEmail_Success()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.Email));
		var email = patientSample.Email;
		var request = new SearchByEmailReq();
		request.Email = email;
		var expectedList = _db.Patient.Where(x => x.Email != null && x.Email.Contains(email)).ToList();

		// Act
		var result = await _patientService.SearchByEmail(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}

	[Fact]
	public async Task SearchByEmail_NoResults()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.Email));
		var email = patientSample.Email;
		var invalidEmail = "__invalid__";
		var request = new SearchByEmailReq();
		request.Email = invalidEmail;

		// Act
		var result = await _patientService.SearchByEmail(request);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public async Task GetActive_Success()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => x.IsActive == true);
		var expectedList = _db.Patient.Where(x => x.IsActive == true).ToList();

		// Act
		var result = await _patientService.GetActive();

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}

	[Fact]
	public async Task GetAllPagingSorting_Success()
	{
		// Arrange
		var patientSample = _db.Patient.First();
		var request = new GetAllPagingSortingReq();
		request.PageSize = 0;
		request.PageOffset = 0;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		var expectedList = _db.Patient.ToList();

		// Act
		var result = await _patientService.GetAllPagingSorting(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Items.Count);
		Assert.Equal(expectedList.Count, result.TotalRowCount);
	}

	[Fact]
	public async Task GetAllPagingSorting_PagingSuccess()
	{
		// Arrange
		var patientSample = _db.Patient.First();
		var request = new GetAllPagingSortingReq();
		request.PageSize = 3;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		var expectedList = _db.Patient.ToList();
		var totalCount = expectedList.Count;
		var lastPgOffset = totalCount == 0 ? 0 : totalCount / request.PageSize;
		if (totalCount % request.PageSize == 0 && totalCount > 0)
			lastPgOffset -= 1;
		var lastPgSize = totalCount == 0 ? 0 : totalCount - (lastPgOffset * request.PageSize);

		// Act
		request.PageOffset = 0;
		var firstPgList = await _patientService.GetAllPagingSorting(request);
		request.PageOffset = lastPgOffset;
		var lastPgList = await _patientService.GetAllPagingSorting(request);

		// Assert
		Assert.True(totalCount == firstPgList.TotalRowCount, $"First total count s/b {totalCount} but was {firstPgList.TotalRowCount}");
		Assert.True(request.PageSize == firstPgList.Items.Count, $"First page size s/b {request.PageSize} but was {firstPgList.Items.Count}");
		Assert.True(totalCount == lastPgList.TotalRowCount, $"Last total count s/b {totalCount} but was {lastPgList.TotalRowCount}");
		Assert.True(lastPgSize == lastPgList.Items.Count, $"Last page size s/b {lastPgSize} but was {lastPgList.Items.Count}");
	}

	[Fact]
	public async Task GetAllPagingSorting_SortingSuccess()
	{
		// Arrange
		var patientSample = _db.Patient.First();
		var request = new GetAllPagingSortingReq();
		request.PageSize = 0;
		request.PageOffset = 0;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		request.SortBy = Patient.PropNames.FirstName;
		var expectedAsc = _db.Patient.ToList().OrderBy(x => x.FirstName).ToList();
		var expectedDesc = _db.Patient.ToList().OrderByDescending(x => x.FirstName).ToList();

		// Act
		request.SortDesc = false;
		var ascResult = await _patientService.GetAllPagingSorting(request);
		request.SortDesc = true;
		var descResult = await _patientService.GetAllPagingSorting(request);

		// Assert
		Assert.Equal(expectedAsc.Select(x => x.Id), ascResult.Items.Select(x => x.Id));
		Assert.Equal(expectedDesc.Select(x => x.Id), descResult.Items.Select(x => x.Id));
	}

	[Fact]
	public async Task SearchActiveLastName_Success()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName) && x.IsActive == true);
		var lastName = patientSample.LastName;
		var expectedList = _db.Patient.Where(x => x.LastName != null && x.LastName.Contains(lastName) && x.IsActive == true).ToList();

		// Act
		var result = await _patientService.SearchActiveLastName(lastName);

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}

	[Fact]
	public async Task SearchActiveLastName_NoResults()
	{
		// Arrange
		var patientSample = _db.Patient.First(x => !string.IsNullOrWhiteSpace(x.LastName) && x.IsActive == true);
		var lastName = patientSample.LastName;
		var invalidLastName = "__invalid__";

		// Act
		var result = await _patientService.SearchActiveLastName(invalidLastName);

		// Assert
		Assert.Empty(result);
	}
}
