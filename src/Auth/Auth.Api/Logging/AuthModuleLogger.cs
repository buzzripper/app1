using App1.Auth.Api;
using Dyvenix.App1.Common.Api.Logging;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App1.Auth.Api.Logging
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
