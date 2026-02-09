using Dyvenix.App1.Common.Data;

namespace Dyvenix.App1.Tests.Integration.Fixtures;

public class TestBase(GlobalTestFixture _fixture) : IAsyncLifetime
{
	protected static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
	protected GlobalTestFixture _fixture = _fixture;
	protected IServiceScope _scope = default!;
	protected App1Db _db = default!;

	public ValueTask InitializeAsync()
	{
		_scope = _fixture.Services.CreateScope();
		_db = _scope.ServiceProvider.GetRequiredService<App1Db>();
		return ValueTask.CompletedTask;
	}

	public ValueTask DisposeAsync()
	{
		_scope.Dispose();
		return ValueTask.CompletedTask;
	}
}