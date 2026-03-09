//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/8/2026 11:54 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
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
	public async Task GetAllRoutes_Success()
	{
		// Arrange
		var clientSample = _db.Client.First();
		var expectedList = _db.Client.ToList();

		// Act
		var result = await _clientService.GetAllRoutes();

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
}
