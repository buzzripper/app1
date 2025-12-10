using Dyvenix.App.Shared.DTOs;

namespace Dyvenix.App.Shared.Interfaces;

public interface IAppSystemService
{
	Task<AppHealthStatus> Health();
}
