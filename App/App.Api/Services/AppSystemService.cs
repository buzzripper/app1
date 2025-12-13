using Dyvenix.App.Shared;
using Dyvenix.App.Shared.DTOs;
using Dyvenix.App.Shared.Interfaces;
using Dyvenix.System.Apis.Extensions;
using Dyvenix.System.Shared.Exceptions;
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
		_logger.Info("APP HEALTH");
		throw new ValidationException("Test exception from AppSystemService.Health");

		return Task.FromResult(new AppHealthStatus
		{
			IsHealthy = true,
			Message = $"{AppConstants.ModuleId} module is healthy",
			Timestamp = DateTime.UtcNow
		});
	}
}
