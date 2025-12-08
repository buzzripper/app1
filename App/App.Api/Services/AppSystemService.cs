using Dyvenix.App.Shared;
using Dyvenix.App.Shared.DTOs;
using Dyvenix.App.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App.Api.Services;

public class AppSystemService : IAppSystemService
{
	private readonly ILogger<AppSystemService> _logger;

	public AppSystemService(ILogger<AppSystemService> logger)
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
