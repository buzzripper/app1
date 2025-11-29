using App1.Shared.DTOs;
using App1.Shared.Interfaces;

namespace App1.Api.Services;

public class SystemService : ISystemService
{
    public Task<HealthStatus> CheckHealth()
    {
        return Task.FromResult(new HealthStatus
        {
            IsHealthy = true,
            Message = "App1 service is healthy",
            Timestamp = DateTime.UtcNow
        });
    }
}
