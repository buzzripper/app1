using Dyvenix.App1.Common.Shared.DTOs;

namespace Dyvenix.App1.Common.Shared.Contracts;

public interface ISystemService
{
    Task<HealthStatus> Health();
    Task<ServiceInfo> GetServiceInfo();
}
