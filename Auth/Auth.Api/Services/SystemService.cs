using Dyvenix.Auth.Shared;
using Dyvenix.Auth.Shared.DTOs;
using Dyvenix.Auth.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Auth.Api.Services;

public class SystemService : IAuthSystemService
{
	private readonly ILogger<SystemService> _logger;

	public SystemService(ILogger<SystemService> logger)
	{
		_logger = logger;
	}

	public Task<string> Alive()
	{
		_logger.LogInformation("Service name requested");
		return Task.FromResult(Constants.ServiceId);
	}

	public Task<AuthHealthStatus> Health()
	{
		return Task.FromResult(new AuthHealthStatus
		{
			IsHealthy = true,
			Message = "Auth service is healthy",
			Timestamp = DateTime.UtcNow
		});
	}
}
