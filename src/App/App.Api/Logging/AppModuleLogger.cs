using Dyvenix.App1.Common.Api.Logging;
using Microsoft.Extensions.Logging;

namespace App1.App.Api.Logging
{
	public interface IAppModuleLogger : IModuleLogger
	{
	}

	public class AppModuleLogger : ModuleLoggerBase, IAppModuleLogger
	{
		public AppModuleLogger(ILoggerFactory loggerFactory) : base(loggerFactory, AppConstants.ModuleId)
		{
		}
	}
}
