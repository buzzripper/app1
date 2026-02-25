
using Dyvenix.App1.Common.Shared.DTOs;

namespace Dyvenix.App1.AdAgent.Shared.Contracts;

public interface IAdAgentSystemService
{
    Task<HealthStatus> Health();
}
