using Dyvenix.App1.App.Shared.Contracts;
using Dyvenix.App1.Common.Shared.Contracts;
using Dyvenix.App1.Common.Shared.DTOs;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App1.App.Api.Services;

public class AppSystemService : IAppSystemService, IManagedService
{
    private readonly ILogger<AppSystemService> _logger;

    public AppSystemService(ILogger<AppSystemService> logger)
    {
        _logger = logger;
    }

    public Task<HealthStatus> Health()
    {
        return Task.FromResult(new HealthStatus
        {
            Status = StatusLevel.Success,
            Message = $"{AppConstants.ModuleId} module is healthy",
            Timestamp = DateTime.UtcNow
        });
    }

    public Task<ServiceInfo> GetServiceInfo()
    {
        var info = new ServiceInfo
        {
            ServiceName = AppConstants.ModuleId,
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
            Version = typeof(AppSystemService).Assembly.GetName().Version?.ToString() ?? "0.0.0",
            HostName = Environment.MachineName,
            Status = StatusLevel.Success,
            StartTimeUtc = DateTime.UtcNow.AddHours(-1), // Example start time
            Uptime = TimeSpan.FromHours(1) // Example uptime
        };
        return Task.FromResult(info);
	}
}

