using Dyvenix.Auth.Shared;
using Dyvenix.Auth.Shared.DTOs;
using Dyvenix.Auth.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dyvenix.Auth.Api.Services;

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
		return Task.FromResult($"{AuthConstants.ServiceId} is alive.");
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
