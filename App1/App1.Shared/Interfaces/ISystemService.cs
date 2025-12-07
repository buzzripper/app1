using Dyvenix.App1.Shared.DTOs;

namespace Dyvenix.App1.Shared.Interfaces;

public interface ISystemService
{
	Task<App1HealthStatus> Health();
}
