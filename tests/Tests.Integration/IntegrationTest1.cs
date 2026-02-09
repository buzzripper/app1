//using Dyvenix.App1.Common.Data;
//using Dyvenix.App1.Tests.Integration.Fixtures;

//namespace Dyvenix.App1.Tests.Integration;

//[Collection(nameof(GlobalTestCollection))]
//public class IntegrationTest1(GlobalTestFixture _fixture) : IAsyncLifetime
//{
//	private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
//	private IServiceScope _scope = default!;
//	private App1Db _db = default!;

//	public ValueTask InitializeAsync()
//	{
//		_scope = _fixture.Services.CreateScope();
//		_db = _scope.ServiceProvider.GetRequiredService<App1Db>();
//		return ValueTask.CompletedTask;
//	}

//	public ValueTask DisposeAsync()
//	{
//		_scope.Dispose();
//		return ValueTask.CompletedTask;
//	}

//	[Fact]
//	public async Task GetWebResourceRootReturnsOkStatusCode()
//	{
//		// Arrange
//		using var httpClient = _fixture.App.CreateHttpClient("app-server");

//		// Act
//		using var response = await httpClient.GetAsync("/api/app/v1/system/ping", TestContext.Current.CancellationToken);

//		if (_db == null)
//			throw new InvalidOperationException("App1Db is not available from the test fixture.");

//		var userCount = _db.AppUser.ToList().Count();
//		TestContext.Current.TestOutputHelper?.WriteLine($"AppUser count: {userCount}");

//		// Assert
//		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//	}
//}
