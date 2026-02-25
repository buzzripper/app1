using App1.Auth.Api;
using Dyvenix.App1.Common.Shared.Contracts;
using Dyvenix.App1.Common.Shared.DTOs;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App1.Auth.Api.Services;

public class AuthSystemService : ISystemService
{
    private readonly ILogger<AuthSystemService> _logger;

    public AuthSystemService(ILogger<AuthSystemService> logger)
    {
        _logger = logger;
    }

    public Task<HealthStatus> Health()
    {
        return Task.FromResult(new HealthStatus
        {

            Status = StatusLevel.Ok,
            Message = $"{AuthConstants.ModuleId} module is healthy",
            Timestamp = DateTime.UtcNow
        });
    }

    public Task<ServiceInfo> GetServiceInfo()
    {
        var info = new ServiceInfo
        {
            ServiceName = AuthConstants.ModuleId,
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
            Version = typeof(AuthSystemService).Assembly.GetName().Version?.ToString() ?? "0.0.0",
            HostName = Environment.MachineName,
            Status = StatusLevel.Ok,
            StartTimeUtc = DateTime.UtcNow.AddHours(-1), // Example start time
            Uptime = TimeSpan.FromHours(1) // Example uptime
        };
        return Task.FromResult(info);
    }
}
