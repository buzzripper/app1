//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/15/2026 7:07 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Linq;
using Dyvenix.App1.Tests.Integration.Data;
using Dyvenix.App1.Tests.Integration.DataSets;
using Dyvenix.App1.Tests.Integration.Fixtures;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Tests.Integration.Tests.Auth.v1;

public class AppUserReadTestFixture(GlobalTestFixture globalFixture) : IAsyncLifetime
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
public class AppUserReadTests : TestBase, IClassFixture<AppUserReadTestFixture>
{
	private readonly AppUserReadTestFixture _fixture;
	private IAppUserService _appUserService = default!;

	public AppUserReadTests(GlobalTestFixture globalFixture, AppUserReadTestFixture fixture)
		: base(globalFixture)
	{
		_fixture = fixture;
	}

	public override async ValueTask InitializeAsync()
	{
		await base.InitializeAsync();
		_appUserService = _scope.ServiceProvider.GetRequiredService<IAppUserService>();
	}

	[Fact]
	public async Task GetById_Success()
	{
		// Arrange
		var appUserSample = _db.AppUser.First();
		var id = appUserSample.Id;

		// Act
		var result = await _appUserService.GetById(id);

		// Assert
		Assert.Equal(appUserSample.Id, result.Id);
	}

	[Fact]
	public async Task GetById_NotFound()
	{
		// Arrange
		var appUserSample = _db.AppUser.First();
		var id = appUserSample.Id;
		var invalidId = Guid.NewGuid();

		// Act
		var result = await _appUserService.GetById(invalidId);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task ReqByUsername_Success()
	{
		// Arrange
		var appUserSample = _db.AppUser.First(x => !string.IsNullOrWhiteSpace(x.Username));
		var username = appUserSample.Username;
		var request = new ReqByUsernameReq();
		request.Username = username;
		var expectedList = _db.AppUser.Where(x => x.Username == username).ToList();

		// Act
		var result = await _appUserService.ReqByUsername(request);

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}

	[Fact]
	public async Task ReqByUsername_NoResults()
	{
		// Arrange
		var appUserSample = _db.AppUser.First(x => !string.IsNullOrWhiteSpace(x.Username));
		var username = appUserSample.Username;
		var invalidUsername = "__invalid__";
		var request = new ReqByUsernameReq();
		request.Username = invalidUsername;

		// Act
		var result = await _appUserService.ReqByUsername(request);

		// Assert
		Assert.Empty(result);
	}
}
