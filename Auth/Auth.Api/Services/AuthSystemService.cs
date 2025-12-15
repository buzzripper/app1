using App1.App.Api.Logging;
using App1.Auth.Shared.Interfaces;
using App1.System.Shared.Exceptions;
using Dyvenix.Auth.Shared.DTOs;
using System;
using System.Threading.Tasks;

namespace App1.Auth.Api.Services;

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
