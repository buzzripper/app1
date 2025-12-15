using App1.System.Apis.Logging;
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
