using Dyvenix.Auth.Api;
using Dyvenix.System.Apis.Logging;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App.Api.Logging
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
