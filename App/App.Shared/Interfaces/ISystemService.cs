using Dyvenix.App.Shared.DTOs;

namespace Dyvenix.App.Shared.Interfaces;

public interface ISystemService
{
	Task<AppHealthStatus> Health();
}
