using Dyvenix.App1.Portal.Server.Logging;
using Dyvenix.System.Apis.Filters;

namespace Dyvenix.App1.Portal.Server.Filters;

public class PortalExceptionFilter<T> : SystemExceptionFilterBase<T>
{
	public PortalExceptionFilter(IPortalModuleLogger logger) : base(logger)
	{
		_moduleName = PortalConstants.ModuleId;
	}
}
