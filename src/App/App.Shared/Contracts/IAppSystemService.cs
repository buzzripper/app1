using Dyvenix.App1.App.Shared.DTOs;

namespace Dyvenix.App1.App.Shared.Contracts
{
	public interface IAppSystemService
	{
		Task<AppHealthStatus> Health();
	}
}
