using Auth.Shared.DTOs;
using Auth.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Auth.Api.Services;

public class SystemService : ISystemService
{
    private readonly ILogger<SystemService> _logger;

    public SystemService(ILogger<SystemService> logger)
    {
        _logger = logger;
    }

    public Task<HealthStatus> CheckHealth()
    {
        return Task.FromResult(new HealthStatus
        {
            IsHealthy = true,
            Message = "Auth service is healthy",
            Timestamp = DateTime.UtcNow
        });
    }

    public Task<string> GetName()
    {
        _logger.LogInformation("Service name requested");
        return Task.FromResult<string>("This service name.");
    }
}
