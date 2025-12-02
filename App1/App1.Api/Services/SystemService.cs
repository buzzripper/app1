using Dyvenix.App1.Shared;
using Dyvenix.App1.Shared.DTOs;
using Dyvenix.App1.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App1.Api.Services;

public class SystemService : IApp1SystemService
{
	private readonly ILogger<SystemService> _logger;

	public SystemService(ILogger<SystemService> logger)
	{
		_logger = logger;
	}

	public Task<string> Alive()
	{
		_logger.LogInformation("Service name requested");
		return Task.FromResult(App1Constants.ServiceId);
	}

	public Task<App1HealthStatus> Health()
	{
		return Task.FromResult(new App1HealthStatus
		{
			IsHealthy = true,
			Message = "App1 service is healthy",
			Timestamp = DateTime.UtcNow
		});
	}
}
