using App1.Auth.Api;
using App1.Auth.Shared.Interfaces;
using Dyvenix.App1.Auth.Api.Logging;
using Dyvenix.App1.Auth.Shared.DTOs;

namespace Dyvenix.App1.Auth.Api.Services;

public class AuthSystemService : IAuthSystemService
{
	private readonly IAuthModuleLogger _logger;

	public AuthSystemService(IAuthModuleLogger logger)
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
