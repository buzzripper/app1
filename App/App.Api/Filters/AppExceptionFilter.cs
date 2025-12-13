using Dyvenix.App.Api.Logging;
using Dyvenix.System.Apis.Filters;

namespace Dyvenix.App.Api.Filters;

public class AppExceptionFilter<T> : SystemExceptionFilterBase<T>
{
	public AppExceptionFilter(IAppModuleLogger logger) : base(logger)
	{
		_moduleName = AppConstants.ModuleId;
	}
}
