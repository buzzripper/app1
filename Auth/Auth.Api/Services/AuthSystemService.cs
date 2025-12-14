using Dyvenix.App.Api.Logging;
using Dyvenix.Auth.Shared.DTOs;
using Dyvenix.Auth.Shared.Interfaces;
using Dyvenix.System.Shared.Exceptions;
using System;
using System.Threading.Tasks;

namespace Dyvenix.Auth.Api.Services;

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
