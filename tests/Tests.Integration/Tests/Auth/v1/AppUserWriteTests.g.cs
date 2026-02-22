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
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Tests.Integration.Tests.Auth.v1;

public class AppUserWriteTestFixture(GlobalTestFixture globalFixture) : IAsyncLifetime
{
	public GlobalTestFixture GlobalFixture { get; } = globalFixture;

	public ValueTask InitializeAsync() => default;

	public ValueTask DisposeAsync() => default;
}

[Collection(nameof(GlobalTestCollection))]
public class AppUserWriteTests : TestBase, IClassFixture<AppUserWriteTestFixture>
{
	private readonly AppUserWriteTestFixture _fixture;
	private IAppUserService _appUserService = default!;

	public AppUserWriteTests(GlobalTestFixture globalFixture, AppUserWriteTestFixture fixture)
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
		_appUserService = _scope.ServiceProvider.GetRequiredService<IAppUserService>();
	}

	[Fact]
	public async Task Create_Success()
	{
		// Arrange
		var newEntity = new AppUser();
		newEntity.Username = "Test_Username_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act
		await _appUserService.CreateAppUser(newEntity);

		// Assert
		Assert.NotEqual(Guid.Empty, newEntity.Id);
	}

	[Fact]
	public async Task Create_ValidationError_MissingRequired()
	{
		// Arrange
		var newEntity = new AppUser();
		newEntity.Username = string.Empty; // Required field left empty

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _appUserService.CreateAppUser(newEntity));
	}

	[Fact]
	public async Task Update_Success()
	{
		// Arrange
		var existing = _db.AppUser.First();
		var entityToUpdate = await _appUserService.GetById(existing.Id);
		Assert.NotNull(entityToUpdate);
		var originalValue = entityToUpdate.Username;
		entityToUpdate.Username = "Test_Username_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act
		await _appUserService.UpdateAppUser(entityToUpdate);

		// Assert
		var updated = _db.AppUser.First(x => x.Id == entityToUpdate.Id);
		Assert.NotNull(updated);
		Assert.NotEqual(originalValue, updated.Username);
	}

	[Fact]
	public async Task Update_NotFound()
	{
		// Arrange
		var entityToUpdate = new AppUser();
		entityToUpdate.Id = Guid.NewGuid();

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _appUserService.UpdateAppUser(entityToUpdate));
	}

	[Fact]
	public async Task Update_ValidationError()
	{
		// Arrange
		var existing = _db.AppUser.First();
		var entityToUpdate = await _appUserService.GetById(existing.Id);
		Assert.NotNull(entityToUpdate);
		entityToUpdate.Username = string.Empty; // Clear required field

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _appUserService.UpdateAppUser(entityToUpdate));
	}

	[Fact]
	public async Task UpdateUsername_Success()
	{
		// Arrange
		var existing = _db.AppUser.First();
		var request = new UpdateUsernameReq();
		request.Id = existing.Id;
		request.Username = "Test_Username_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act
		await _appUserService.UpdateUsername(request);

		// Assert
	}

	[Fact]
	public async Task UpdateUsername_NotFound()
	{
		// Arrange
		var request = new UpdateUsernameReq();
		request.Id = Guid.NewGuid();
		request.Username = "Test_Username_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _appUserService.UpdateUsername(request));
	}

	[Fact]
	public async Task UpdateUsername_ValidationError()
	{
		// Arrange
		var existing = _db.AppUser.First();
		var request = new UpdateUsernameReq();
		request.Id = existing.Id;
		request.Username = string.Empty; // Required field empty

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _appUserService.UpdateUsername(request));
	}

	[Fact]
	public async Task Delete_Success()
	{
		// Arrange
		var existing = _db.AppUser.First();
		var idToDelete = existing.Id;

		// Act
		await _appUserService.DeleteAppUser(idToDelete);

		// Assert
		var deleted = await _appUserService.GetById(idToDelete);
		Assert.Null(deleted);
	}

	[Fact]
	public async Task Delete_NotFound()
	{
		// Arrange
		var invalidId = Guid.NewGuid();

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _appUserService.DeleteAppUser(invalidId));
	}
}
