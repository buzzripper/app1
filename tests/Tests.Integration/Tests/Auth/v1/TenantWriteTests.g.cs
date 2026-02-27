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

public class TenantWriteTestFixture(GlobalTestFixture globalFixture) : IAsyncLifetime
{
	public GlobalTestFixture GlobalFixture { get; } = globalFixture;

	public ValueTask InitializeAsync() => default;

	public ValueTask DisposeAsync() => default;
}

[Collection(nameof(GlobalTestCollection))]
public class TenantWriteTests : TestBase, IClassFixture<TenantWriteTestFixture>
{
	private readonly TenantWriteTestFixture _fixture;
	private ITenantService _tenantService = default!;

	public TenantWriteTests(GlobalTestFixture globalFixture, TenantWriteTestFixture fixture)
		: base(globalFixture)
	{
		_fixture = fixture;
	}

	public override async ValueTask InitializeAsync()
	{
		await base.InitializeAsync();
		// Reset database before each write test for isolation
		var dataManager = _scope.ServiceProvider.GetRequiredService<IDataManager>();
		await dataManager.Reset(DataSetType.Main.ToString());
		_tenantService = _scope.ServiceProvider.GetRequiredService<ITenantService>();
	}

	[Fact]
	public async Task Create_Success()
	{
		// Arrange
		var newEntity = new Tenant();
		newEntity.Name = "Test_Name_" + Guid.NewGuid().ToString().Substring(0, 8);
		newEntity.Slug = "Test_Slug_" + Guid.NewGuid().ToString().Substring(0, 8);
		newEntity.AuthMode = default(AuthMode);
		newEntity.IsActive = true;
		newEntity.CreatedAt = DateTime.UtcNow;
		newEntity.ExternalAuthority = "Test_ExternalAuthority_" + Guid.NewGuid().ToString().Substring(0, 8);
		newEntity.ExternalClientId = "Test_ExternalClientId_" + Guid.NewGuid().ToString().Substring(0, 8);
		newEntity.ExternalClientSecret = "Test_ExternalClientSecret_" + Guid.NewGuid().ToString().Substring(0, 8);
		newEntity.ADDcHost = "Test_ADDcHost_" + Guid.NewGuid().ToString().Substring(0, 8);
		newEntity.ADDomain = "Test_ADDomain_" + Guid.NewGuid().ToString().Substring(0, 8);
		newEntity.ADLdapPort = 42;
		newEntity.ADBaseDn = "Test_ADBaseDn_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act
		await _tenantService.CreateTenant(newEntity);

		// Assert
		Assert.NotEqual(Guid.Empty, newEntity.Id);
	}

	[Fact]
	public async Task Create_ValidationError_MissingRequired()
	{
		// Arrange
		var newEntity = new Tenant();
		newEntity.Name = string.Empty; // Required field left empty

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _tenantService.CreateTenant(newEntity));
	}

	[Fact]
	public async Task Update_Success()
	{
		// Arrange
		var existing = _db.Tenant.First();
		var entityToUpdate = await _tenantService.GetById(existing.Id);
		Assert.NotNull(entityToUpdate);
		var originalValue = entityToUpdate.Name;
		entityToUpdate.Name = "Test_Name_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act
		await _tenantService.UpdateTenant(entityToUpdate);

		// Assert
		var updated = _db.Tenant.First(x => x.Id == entityToUpdate.Id);
		Assert.NotNull(updated);
		Assert.NotEqual(originalValue, updated.Name);
	}

	[Fact]
	public async Task Update_NotFound()
	{
		// Arrange
		var entityToUpdate = new Tenant();
		entityToUpdate.Id = Guid.NewGuid();

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _tenantService.UpdateTenant(entityToUpdate));
	}

	[Fact]
	public async Task Update_ValidationError()
	{
		// Arrange
		var existing = _db.Tenant.First();
		var entityToUpdate = await _tenantService.GetById(existing.Id);
		Assert.NotNull(entityToUpdate);
		entityToUpdate.Name = string.Empty; // Clear required field

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _tenantService.UpdateTenant(entityToUpdate));
	}

	[Fact]
	public async Task UpdateName_Success()
	{
		// Arrange
		var existing = _db.Tenant.First();
		var request = new UpdateNameReq();
		request.Id = existing.Id;
		request.Name = "Test_Name_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act
		await _tenantService.UpdateName(request);

		// Assert
	}

	[Fact]
	public async Task UpdateName_NotFound()
	{
		// Arrange
		var request = new UpdateNameReq();
		request.Id = Guid.NewGuid();
		request.Name = "Test_Name_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _tenantService.UpdateName(request));
	}

	[Fact]
	public async Task UpdateName_ValidationError()
	{
		// Arrange
		var existing = _db.Tenant.First();
		var request = new UpdateNameReq();
		request.Id = existing.Id;
		request.Name = string.Empty; // Required field empty

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _tenantService.UpdateName(request));
	}

	[Fact]
	public async Task Delete_Success()
	{
		// Arrange
		var existing = _db.Tenant.First();
		var idToDelete = existing.Id;

		// Act
		await _tenantService.DeleteTenant(idToDelete);

		// Assert
		var deleted = await _tenantService.GetById(idToDelete);
		Assert.Null(deleted);
	}

	[Fact]
	public async Task Delete_NotFound()
	{
		// Arrange
		var invalidId = Guid.NewGuid();

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _tenantService.DeleteTenant(invalidId));
	}
}
