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

	//public Task<string> Ping()
	//{
	//	return Task.FromResult($"{this.GetType().Name} ({DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC)");
	//}

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
