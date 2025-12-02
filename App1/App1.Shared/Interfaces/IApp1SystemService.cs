using Dyvenix.App1.Shared.DTOs;

namespace Dyvenix.App1.Shared.Interfaces;

public interface IApp1SystemService
{
	Task<string> Alive();
	Task<App1HealthStatus> Health();
}
