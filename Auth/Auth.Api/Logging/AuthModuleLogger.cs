using App1.System.Apis.Logging;
using Microsoft.Extensions.Logging;

namespace App1.Auth.Api.Logging
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
