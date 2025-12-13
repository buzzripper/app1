using Dyvenix.System.Apis.Logging;

namespace Dyvenix.App1.Portal.Server.Logging
{
	public interface IPortalModuleLogger : IModuleLogger
	{
	}

	public class PortalModuleLogger : ModuleLoggerBase, IPortalModuleLogger
	{
		public PortalModuleLogger(ILoggerFactory loggerFactory) : base(loggerFactory, PortalConstants.ModuleId)
		{
		}
	}
}
