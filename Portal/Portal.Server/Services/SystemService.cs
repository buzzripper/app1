using Dyvenix.App1.Portal.Server.DTOs;
using Dyvenix.App1.Portal.Server.Interfaces;
using Dyvenix.System.Apis.Extensions;

namespace Dyvenix.App1.Portal.Server.Services;

public class SystemService : ISystemService
{
	private readonly ILogger<SystemService> _logger;

	public SystemService(ILogger<SystemService> logger)
	{
		_logger = logger;
	}

	public Task<PortalHealthStatus> Health()
	{
		_logger.Info("PORTAL HEALTH");

		return Task.FromResult(new PortalHealthStatus
		{
			IsHealthy = true,
			Message = $"{PortalConstants.ModuleId} module is healthy",
			Timestamp = DateTime.UtcNow
		});
	}
}
