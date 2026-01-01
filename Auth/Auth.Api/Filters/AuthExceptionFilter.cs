using App1.App.Api.Logging;
using App1.Auth.Api;
using App1.System.Apis.Filters;
using App1.System.Apis.Logging;

namespace App1.App.Api.Filters;

public class AuthExceptionFilter<T> : SystemExceptionFilterBase<T>
{
	public AuthExceptionFilter(IAuthModuleLogger logger) : base(logger)
	{
		_moduleName = AuthConstants.ModuleId;
	}
}
