using App1.Shared.DTOs;

namespace App1.Shared.Interfaces;

public interface ISystemService
{
    Task<HealthStatus> CheckHealth();
}
