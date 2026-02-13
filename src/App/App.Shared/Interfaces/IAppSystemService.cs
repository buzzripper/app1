namespace Dyvenix.App1.App.Shared.Interfaces;

public interface IAppSystemService
{
	Task<AppHealthStatus> Health();
}
