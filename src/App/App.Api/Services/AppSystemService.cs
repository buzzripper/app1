using App1.App.Shared.DTOs;
using App1.App.Shared.Interfaces;
using Dyvenix.App1.App.Api.Logging;

namespace Dyvenix.App1.App.Api.Services;

public class AppSystemService : IAppSystemService
{
	private readonly IAppModuleLogger _logger;

	public AppSystemService(IAppModuleLogger logger)
	{
		_logger = logger;
	}

	public Task<AppHealthStatus> Health()
	{
		return Task.FromResult(new AppHealthStatus
		{
			IsHealthy = true,
			Message = $"{AppConstants.ModuleId} module is healthy",
			Timestamp = DateTime.UtcNow
		});
	}
}

