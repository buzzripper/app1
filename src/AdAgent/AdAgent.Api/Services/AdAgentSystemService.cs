using Dyvenix.App1.AdAgent.Api;
using Dyvenix.App1.AdAgent.Shared.Contracts;
using Dyvenix.App1.Common.Shared.DTOs;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App1.App.Api.Services;

public class AdAgentSystemService : IAdAgentSystemService
{
    private readonly ILogger<AdAgentSystemService> _logger;

    public AdAgentSystemService(ILogger<AdAgentSystemService> logger)
    {
        _logger = logger;
    }

    public Task<HealthStatus> Health()
    {
        return Task.FromResult(new HealthStatus
        {
            Status = StatusLevel.Success,
            Message = $"{AdAgentConstants.ModuleId} module is healthy",
            Timestamp = DateTime.UtcNow
        });
    }
}

