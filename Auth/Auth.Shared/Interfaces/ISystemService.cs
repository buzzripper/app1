using Auth.Shared.DTOs;

namespace Auth.Shared.Interfaces;

public interface ISystemService
{
    Task<HealthStatus> CheckHealth();
}
