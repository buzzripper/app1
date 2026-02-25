using App1.App1.Portal.Server;
using App1.App1.Portal.Server.Interfaces;
using Dyvenix.App1.Common.Shared.DTOs;
using Dyvenix.App1.Portal.Server.DTOs;
using Dyvenix.App1.Portal.Server.Logging;

namespace Dyvenix.App1.Portal.Server.Services;

public class PortalSystemService : IPortalSystemService
{
    private readonly IPortalModuleLogger _logger;

    public PortalSystemService(IPortalModuleLogger logger)
    {
        _logger = logger;
    }

    public Task<PortalHealthStatus> Health()
    {
        return Task.FromResult(new PortalHealthStatus
        {
            Status = StatusLevel.Success,
            Message = $"{PortalConstants.ModuleId} module is healthy",
            Timestamp = DateTime.UtcNow
        });
    }
}
