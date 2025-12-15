using App1.Portal.Server.Logging;
using App1.System.Apis.Filters;

namespace App1.Portal.Server.Filters;

public class PortalExceptionFilter<T> : SystemExceptionFilterBase<T>
{
	public PortalExceptionFilter(IPortalModuleLogger logger) : base(logger)
	{
		_moduleName = PortalConstants.ModuleId;
	}
}
