using Dyvenix.App1.Tests.Integration.Fixtures;

namespace Dyvenix.App1.Tests.Integration.App;

[Collection(nameof(GlobalTestCollection))]
public class PatientTests(GlobalTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task Ping()
	{
		// Arrange
		using var httpClient = _fixture.App.CreateHttpClient("app-server");

		// Act
		using var response = await httpClient.GetAsync("/api/app/v1/system/ping", TestContext.Current.CancellationToken);

		if (_db == null)
			throw new InvalidOperationException("App1Db is not available from the test fixture.");

		var userCount = _db.Patient.ToList().Count();
		TestContext.Current.TestOutputHelper?.WriteLine($"Patient count: {userCount}");

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}
}
