using App1.Portal.Server.DTOs;
using App1.Portal.Server.Interfaces;
using App1.Portal.Server.Logging;

namespace App1.Portal.Server.Services;

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
			IsHealthy = true,
			Message = $"{PortalConstants.ModuleId} module is healthy",
			Timestamp = DateTime.UtcNow
		});
	}
}
