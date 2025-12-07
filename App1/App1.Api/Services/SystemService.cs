using Dyvenix.App1.Shared;
using Dyvenix.App1.Shared.DTOs;
using Dyvenix.App1.Shared.Interfaces;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App1.Api.Services;

public class SystemService : ISystemService
{
	private readonly ILogger<SystemService> _logger;

	public SystemService(ILogger<SystemService> logger)
	{
		_logger = logger;
	}

	public Task<App1HealthStatus> Health()
	{
		return Task.FromResult(new App1HealthStatus
		{
			IsHealthy = true,
			Message = $"{App1Constants.ModuleId} module is healthy",
			Timestamp = DateTime.UtcNow
		});
	}
}
