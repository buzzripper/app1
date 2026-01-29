using App1.Auth.Api;
using Dyvenix.App1.Auth.Api.Logging;
using Dyvenix.App1.Common.Api.Filters;

namespace Dyvenix.App1.Auth.Api.Filters;

public class AuthExceptionFilter<T> : SystemExceptionFilterBase<T>
{
	public AuthExceptionFilter(IAuthModuleLogger logger) : base(logger)
	{
		_moduleName = AuthConstants.ModuleId;
	}
}
