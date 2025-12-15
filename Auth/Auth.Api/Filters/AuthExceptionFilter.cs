using App1.Auth.Api.Logging;
using App1.System.Apis.Filters;

namespace App1.Auth.Api.Filters;

public class AuthExceptionFilter<T> : SystemExceptionFilterBase<T>
{
	public AuthExceptionFilter(IAuthModuleLogger logger) : base(logger)
	{
		_moduleName = AuthConstants.ModuleId;
	}
}
