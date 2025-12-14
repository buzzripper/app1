using Dyvenix.App.Api.Logging;
using Dyvenix.App.Shared.DTOs;
using Dyvenix.App.Shared.Interfaces;
using Dyvenix.System.Shared.Exceptions;

namespace Dyvenix.App.Api.Services;

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

