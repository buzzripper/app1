using App1.App.Api.Logging;
using App1.System.Apis.Filters;

namespace App1.App.Api.Filters;

public class AppExceptionFilter<T> : SystemExceptionFilterBase<T>
{
	public AppExceptionFilter(IAppModuleLogger logger) : base(logger)
	{
		_moduleName = AppConstants.ModuleId;
	}
}
