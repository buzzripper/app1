using Auth.Shared.DTOs;
using Auth.Shared.Interfaces;

namespace Auth.Api.Services;

public class SystemService : ISystemService
{
    public Task<HealthStatus> CheckHealth()
    {
        return Task.FromResult(new HealthStatus
        {
            IsHealthy = true,
            Message = "Auth service is healthy",
            Timestamp = DateTime.UtcNow
        });
    }
}
