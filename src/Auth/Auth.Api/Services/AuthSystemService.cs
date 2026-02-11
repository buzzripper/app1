using App1.Auth.Api;
using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App1.Auth.Api.Services;

public class AuthSystemService : IAuthSystemService
{
	private readonly ILogger<AuthSystemService> _logger;

	public AuthSystemService(ILogger<AuthSystemService> logger)
	{
		_logger = logger;
	}

	public Task<AuthHealthStatus> Health()
	{
		return Task.FromResult(new AuthHealthStatus
		{
			IsHealthy = true,
			Message = $"{AuthConstants.ModuleId} module is healthy",
			Timestamp = DateTime.UtcNow
		});
	}
}
