//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/1/2026 10:25 PM. Any changes made to it will be lost.
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
	public async Task GetById_Success()
	{
		// Arrange
		var clientSample = _db.Client.First();
		var id = clientSample.Id;

		// Act
		var result = await _clientService.GetById(id);

		// Assert
		Assert.Equal(clientSample.Id, result.Id);
	}

	[Fact]
	public async Task GetById_NotFound()
	{
		// Arrange
		var clientSample = _db.Client.First();
		var id = clientSample.Id;
		var invalidId = Guid.NewGuid();

		// Act
		var result = await _clientService.GetById(invalidId);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetByKey_Success()
	{
		// Arrange
		var clientSample = _db.Client.First(x => !string.IsNullOrWhiteSpace(x.Key));
		var key = clientSample.Key;

		// Act
		var result = await _clientService.GetByKey(key);

		// Assert
		Assert.Equal(clientSample.Id, result.Id);
	}

	[Fact]
	public async Task GetByKey_NotFound()
	{
		// Arrange
		var clientSample = _db.Client.First(x => !string.IsNullOrWhiteSpace(x.Key));
		var key = clientSample.Key;
		var invalidKey = "__invalid__";

		// Act
		var result = await _clientService.GetByKey(invalidKey);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetAllClientOptions_Success()
	{
		// Arrange
		var clientSample = _db.Client.First();
		var request = new GetAllClientOptionsReq();
		var expectedList = _db.Client.ToList();

		// Act
		var result = await _clientService.GetAllClientOptions(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}

	[Fact]
	public async Task GetAllClientOptions_SortingSuccess()
	{
		// Arrange
		var clientSample = _db.Client.First();
		var request = new GetAllClientOptionsReq();
		request.SortBy = Client.PropNames.Key;
		var expectedAsc = _db.Client.ToList().OrderBy(x => x.Key).ToList();
		var expectedDesc = _db.Client.ToList().OrderByDescending(x => x.Key).ToList();

		// Act
		request.SortDesc = false;
		var ascResult = await _clientService.GetAllClientOptions(request);
		request.SortDesc = true;
		var descResult = await _clientService.GetAllClientOptions(request);

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
}
