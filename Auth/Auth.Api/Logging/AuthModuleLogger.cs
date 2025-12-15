using App1.Auth.Api;
using App1.System.Apis.Logging;
using Microsoft.Extensions.Logging;

namespace App1.App.Api.Logging
{
	public interface IAuthModuleLogger : IModuleLogger
	{
	}

	public class AuthModuleLogger : ModuleLoggerBase, IAuthModuleLogger
	{
		public AuthModuleLogger(ILoggerFactory loggerFactory) : base(loggerFactory, AuthConstants.ModuleId)
		{
		}
	}
}
