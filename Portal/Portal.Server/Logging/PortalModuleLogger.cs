using App1.App1.Portal.Server;
using Dyvenix.App1.Common.Api.Logging;

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
