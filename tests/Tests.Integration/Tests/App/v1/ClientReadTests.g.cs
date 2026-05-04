using System;
using System.Linq;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.Tests.Integration.Data;
using Dyvenix.App1.Tests.Integration.DataSets;
using Dyvenix.App1.Tests.Integration.Fixtures;
using Dyvenix.App1.App.Api.Entities;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;

namespace Dyvenix.App1.Tests.Integration.Tests.App.v1;

public class ClientReadTestFixture(GlobalTestFixture globalFixture) : IAsyncLifetime
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
public class ClientReadTests : TestBase, IClassFixture<ClientReadTestFixture>
{
	private readonly ClientReadTestFixture _fixture;
	private IClientService _clientService = default!;

	public ClientReadTests(GlobalTestFixture globalFixture, ClientReadTestFixture fixture)
		: base(globalFixture)
	{
		_fixture = fixture;
	}

	public override async ValueTask InitializeAsync()
	{
		await base.InitializeAsync();
		_clientService = _scope.ServiceProvider.GetRequiredService<IClientService>();
	}

	[Fact]
	public async Task GetClientById_Success()
	{
		// Arrange
		var clientSample = _db.Client.First();
		var id = clientSample.Id;

		// Act
		var result = await _clientService.GetClientById(id);

		// Assert
		Assert.Equal(clientSample.Id, result.Id);
	}

	[Fact]
	public async Task GetClientById_NotFound()
	{
		// Arrange
		var clientSample = _db.Client.First();
		var id = clientSample.Id;
		var invalidId = Guid.NewGuid();

		// Act
		var result = await _clientService.GetClientById(invalidId);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetClientByKey_Success()
	{
		// Arrange
		var clientSample = _db.Client.First(x => !string.IsNullOrWhiteSpace(x.Key));
		var key = clientSample.Key;

		// Act
		var result = await _clientService.GetClientByKey(key);

		// Assert
		Assert.Equal(clientSample.Id, result.Id);
	}

	[Fact]
	public async Task GetClientByKey_NotFound()
	{
		// Arrange
		var clientSample = _db.Client.First(x => !string.IsNullOrWhiteSpace(x.Key));
		var key = clientSample.Key;
		var invalidKey = "__invalid__";

		// Act
		var result = await _clientService.GetClientByKey(invalidKey);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetAllClientLookupItems_Success()
	{
		// Arrange
		var clientSample = _db.Client.First();
		var request = new GetAllClientLookupItemsReq();
		var expectedList = _db.Client.ToList();

		// Act
		var result = await _clientService.GetAllClientLookupItems(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}

	[Fact]
	public async Task GetAllClientLookupItems_SortingSuccess()
	{
		// Arrange
		var clientSample = _db.Client.First();
		var request = new GetAllClientLookupItemsReq();
		request.SortBy = Client.PropNames.Key;
		var expectedAsc = _db.Client.ToList().OrderBy(x => x.Key).ToList();
		var expectedDesc = _db.Client.ToList().OrderByDescending(x => x.Key).ToList();

		// Act
		request.SortDesc = false;
		var ascResult = await _clientService.GetAllClientLookupItems(request);
		request.SortDesc = true;
		var descResult = await _clientService.GetAllClientLookupItems(request);

		// Assert
		Assert.Equal(expectedAsc.Select(x => x.Id), ascResult.Select(x => x.Id));
		Assert.Equal(expectedDesc.Select(x => x.Id), descResult.Select(x => x.Id));
	}

	[Fact]
	public async Task GetAllClientRoutes_Success()
	{
		// Arrange
		var clientSample = _db.Client.First();
		var expectedList = _db.Client.ToList();

		// Act
		var result = await _clientService.GetAllClientRoutes();

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}

	[Fact]
	public async Task GetAllClients_Success()
	{
		// Arrange
		var clientSample = _db.Client.First();
		var request = new GetAllClientsReq();
		var expectedList = _db.Client.ToList();

		// Act
		var result = await _clientService.GetAllClients(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}

	[Fact]
	public async Task GetAllClients_SortingSuccess()
	{
		// Arrange
		var clientSample = _db.Client.First();
		var request = new GetAllClientsReq();
		request.SortBy = Client.PropNames.Key;
		var expectedAsc = _db.Client.ToList().OrderBy(x => x.Key).ToList();
		var expectedDesc = _db.Client.ToList().OrderByDescending(x => x.Key).ToList();

		// Act
		request.SortDesc = false;
		var ascResult = await _clientService.GetAllClients(request);
		request.SortDesc = true;
		var descResult = await _clientService.GetAllClients(request);

		// Assert
		Assert.Equal(expectedAsc.Select(x => x.Id), ascResult.Select(x => x.Id));
		Assert.Equal(expectedDesc.Select(x => x.Id), descResult.Select(x => x.Id));
	}

	[Fact]
	public async Task SearchClientsByName_Success()
	{
		// Arrange
		var clientSample = _db.Client.First(x => !string.IsNullOrWhiteSpace(x.Name));
		var name = clientSample.Name;
		var request = new SearchClientsByNameReq();
		request.PageSize = 0;
		request.PageOffset = 0;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		request.Name = name;
		var expectedList = _db.Client.Where(x => x.Name == name).ToList();

		// Act
		var result = await _clientService.SearchClientsByName(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Items.Count);
		Assert.Equal(expectedList.Count, result.TotalRowCount);
	}

	[Fact]
	public async Task SearchClientsByName_NoResults()
	{
		// Arrange
		var clientSample = _db.Client.First(x => !string.IsNullOrWhiteSpace(x.Name));
		var name = clientSample.Name;
		var invalidName = "__invalid__";
		var request = new SearchClientsByNameReq();
		request.PageSize = 0;
		request.PageOffset = 0;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		request.Name = invalidName;

		// Act
		var result = await _clientService.SearchClientsByName(request);

		// Assert
		Assert.Empty(result.Items);
	}

	[Fact]
	public async Task SearchClientsByName_PagingSuccess()
	{
		// Arrange
		var clientSample = _db.Client.First(x => !string.IsNullOrWhiteSpace(x.Name));
		var name = clientSample.Name;
		var request = new SearchClientsByNameReq();
		request.Name = name;
		request.PageSize = 3;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		var expectedList = _db.Client.Where(x => x.Name == name).ToList();
		var totalCount = expectedList.Count;
		var lastPgOffset = totalCount == 0 ? 0 : totalCount / request.PageSize;
		if (totalCount % request.PageSize == 0 && totalCount > 0)
			lastPgOffset -= 1;
		var lastPgSize = totalCount == 0 ? 0 : totalCount - (lastPgOffset * request.PageSize);

		// Act
		request.PageOffset = 0;
		var firstPgList = await _clientService.SearchClientsByName(request);
		request.PageOffset = lastPgOffset;
		var lastPgList = await _clientService.SearchClientsByName(request);

		// Assert
		Assert.True(totalCount == firstPgList.TotalRowCount, $"First total count s/b {totalCount} but was {firstPgList.TotalRowCount}");
		Assert.True(request.PageSize == firstPgList.Items.Count, $"First page size s/b {request.PageSize} but was {firstPgList.Items.Count}");
		Assert.True(totalCount == lastPgList.TotalRowCount, $"Last total count s/b {totalCount} but was {lastPgList.TotalRowCount}");
		Assert.True(lastPgSize == lastPgList.Items.Count, $"Last page size s/b {lastPgSize} but was {lastPgList.Items.Count}");
	}

	[Fact]
	public async Task SearchClientsByName_SortingSuccess()
	{
		// Arrange
		var clientSample = _db.Client.First(x => !string.IsNullOrWhiteSpace(x.Name));
		var name = clientSample.Name;
		var request = new SearchClientsByNameReq();
		request.PageSize = 0;
		request.PageOffset = 0;
		request.RecalcRowCount = true;
		request.GetRowCountOnly = false;
		request.Name = name;
		request.SortBy = Client.PropNames.Key;
		var expectedAsc = _db.Client.Where(x => x.Name == name).ToList().OrderBy(x => x.Key).ToList();
		var expectedDesc = _db.Client.Where(x => x.Name == name).ToList().OrderByDescending(x => x.Key).ToList();

		// Act
		request.SortDesc = false;
		var ascResult = await _clientService.SearchClientsByName(request);
		request.SortDesc = true;
		var descResult = await _clientService.SearchClientsByName(request);

		// Assert
		Assert.Equal(expectedAsc.Select(x => x.Id), ascResult.Items.Select(x => x.Id));
		Assert.Equal(expectedDesc.Select(x => x.Id), descResult.Items.Select(x => x.Id));
	}
}
