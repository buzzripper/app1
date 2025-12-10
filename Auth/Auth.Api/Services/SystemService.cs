using Dyvenix.Auth.Shared;
using Dyvenix.Auth.Shared.DTOs;
using Dyvenix.Auth.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dyvenix.Auth.Api.Services;

public class SystemService : ISystemService
{
	private readonly ILogger<SystemService> _logger;

	public SystemService(ILogger<SystemService> logger)
	{
		_logger = logger;
	}

	public Task<AuthHealthStatus> Health()
	{
		_logger.LogInformation("+++++++++++++++++++++   AUTH HEALTH  ++++++++++++++++++++++++");

		return Task.FromResult(new AuthHealthStatus
		{
			IsHealthy = true,
			Message = $"{AuthConstants.ModuleId} module is healthy",
			Timestamp = DateTime.UtcNow
		});
	}
}
