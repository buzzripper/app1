using App1.Auth.Api;
using Dyvenix.App1.Auth.Shared.Contracts;
using Dyvenix.App1.Common.Shared.DTOs;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App1.Auth.Api.Services;

public class AuthSystemService : IAuthSystemService
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
            Status = StatusLevel.Success,
            Message = $"{AuthConstants.ModuleId} module is healthy",
            Timestamp = DateTime.UtcNow
        });
    }
}
