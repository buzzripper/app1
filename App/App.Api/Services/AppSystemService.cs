using App1.App.Api.Logging;
using App1.App.Shared.DTOs;
using App1.App.Shared.Interfaces;
using App1.System.Shared.Context;

namespace App1.App.Api.Services;

public class AppSystemService(IAppModuleLogger _logger, IRequestContext _requestContext) : IAppSystemService
{
	public Task<AppHealthStatus> Health()
	{
		return Task.FromResult(new AppHealthStatus
		{
			IsHealthy = true,
			Message = $"{AppConstants.ModuleId} module is healthy. User is {_requestContext.UserId}",
			Timestamp = DateTime.UtcNow
		});
	}
}

