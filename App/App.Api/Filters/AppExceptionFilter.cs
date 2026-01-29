using App1.App.Api.Logging;
using Dyvenix.App1.Common.Api.Filters;

namespace App1.App.Api.Filters;

public class AppExceptionFilter<T> : SystemExceptionFilterBase<T>
{
	public AppExceptionFilter(IAppModuleLogger logger) : base(logger)
	{
		_moduleName = AppConstants.ModuleId;
	}
}
