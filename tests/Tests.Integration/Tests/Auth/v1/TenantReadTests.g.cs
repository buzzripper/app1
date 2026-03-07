//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/27/2026 4:53 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Linq;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.Auth.Tests.Integration.Data;
using Dyvenix.Auth.Tests.Integration.DataSets;
using Dyvenix.Auth.Tests.Integration.Fixtures;
using Dyvenix.App1.Auth.Data.Entities;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.Auth.Tests.Integration.Tests.Auth.v1;

public class TenantReadTestFixture(GlobalTestFixture globalFixture) : IAsyncLifetime
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
public class TenantReadTests : TestBase, IClassFixture<TenantReadTestFixture>
{
	private readonly TenantReadTestFixture _fixture;
	private ITenantService _tenantService = default!;

	public TenantReadTests(GlobalTestFixture globalFixture, TenantReadTestFixture fixture)
		: base(globalFixture)
	{
		_fixture = fixture;
	}

	public override async ValueTask InitializeAsync()
	{
		await base.InitializeAsync();
		_tenantService = _scope.ServiceProvider.GetRequiredService<ITenantService>();
	}

	[Fact]
	public async Task GetById_Success()
	{
		// Arrange
		var tenantSample = _db.Tenant.First();
		var id = tenantSample.Id;

		// Act
		var result = await _tenantService.GetById(id);

		// Assert
		Assert.Equal(tenantSample.Id, result.Id);
	}

	[Fact]
	public async Task GetById_NotFound()
	{
		// Arrange
		var tenantSample = _db.Tenant.First();
		var id = tenantSample.Id;
		var invalidId = Guid.NewGuid();

		// Act
		var result = await _tenantService.GetById(invalidId);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetAll_Success()
	{
		// Arrange
		var tenantSample = _db.Tenant.First();
		var expectedList = _db.Tenant.ToList();

		// Act
		var result = await _tenantService.GetAll();

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}
}
