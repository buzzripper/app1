using Dyvenix.App.Shared;
using Dyvenix.App.Shared.DTOs;
using Dyvenix.App.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App.Api.Services;

public class SystemService : ISystemService
{
	private readonly ILogger<SystemService> _logger;

	public SystemService(ILogger<SystemService> logger)
	{
		_logger = logger;
	}

	public Task<AppHealthStatus> Health()
	{
		_logger.LogInformation("+++++++++++++++++++++   APP HEALTH  ++++++++++++++++++++++++");

		return Task.FromResult(new AppHealthStatus
		{
			IsHealthy = true,
			Message = $"{AppConstants.ModuleId} module is healthy",
			Timestamp = DateTime.UtcNow
		});
	}
}
