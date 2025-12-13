using Dyvenix.App.Api.Logging;
using Dyvenix.Auth.Api;
using Dyvenix.System.Apis.Filters;
using Dyvenix.System.Apis.Logging;

namespace Dyvenix.App.Api.Filters;

public class AuthExceptionFilter<T> : SystemExceptionFilterBase<T>
{
	public AuthExceptionFilter(IAuthModuleLogger logger) : base(logger)
	{
		_moduleName = AuthConstants.ModuleId;
	}
}
