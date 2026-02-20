using Dyvenix.App1.Common.Data;
using Dyvenix.App1.Tests.Integration.Authorization;

namespace Dyvenix.App1.Tests.Integration.Fixtures;

public class TestBase(GlobalTestFixture _globalFixture) : IAsyncLifetime
{
	protected static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
	protected GlobalTestFixture _globalFixture = _globalFixture;
	protected IServiceScope _scope = default!;
	protected App1Db _db = default!;

	public virtual async ValueTask InitializeAsync()
	{
		_scope = _globalFixture.Services.CreateScope();
		_db = _scope.ServiceProvider.GetRequiredService<App1Db>();
		await ValueTask.CompletedTask;
	}

	protected void SetPermissions(params string[] permissions)
	{
		_scope.ServiceProvider.GetRequiredService<TestAuthContext>().SetPermissions(permissions);
	}

	public ValueTask DisposeAsync()
	{
		_scope.Dispose();
		return ValueTask.CompletedTask;
	}
}