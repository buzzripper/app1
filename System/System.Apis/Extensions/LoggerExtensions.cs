using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Dyvenix.System.Apis.Extensions;

public static class LoggerExtensions
{
	public static void Error(this ILogger logger, Exception ex, [CallerFilePath] string sourceClass = "", [CallerMemberName] string sourceMethod = "")
	{
		var message = ex.GetBaseException().Message;
		Log(LogLevel.Error, logger, message, ParseSourceClass(sourceClass), sourceMethod);
	}

	public static void Warn(this ILogger logger, string message, [CallerFilePath] string sourceClass = "", [CallerMemberName] string sourceMethod = "")
	{
		Log(LogLevel.Warning, logger, message, ParseSourceClass(sourceClass), sourceMethod);
	}

	public static void Info(this ILogger logger, string message, [CallerFilePath] string sourceClass = "", [CallerMemberName] string sourceMethod = "")
	{
		Log(LogLevel.Information, logger, message, ParseSourceClass(sourceClass), sourceMethod);
	}

	public static void Debug(this ILogger logger, string message, [CallerFilePath] string sourceClass = "", [CallerMemberName] string sourceMethod = "")
	{
		Log(LogLevel.Debug, logger, message, ParseSourceClass(sourceClass), sourceMethod);
	}

	private static string ParseSourceClass(string sourceClass)
	{
		// If it contains '.', it's likely a filepath - extract filename without extension
		if (sourceClass.Contains('.'))
			return Path.GetFileNameWithoutExtension(sourceClass);

		return sourceClass;
	}

	private static void Log(LogLevel logLevel, ILogger logger, string message, string sourceClass = "", string sourceMethod = "")
	{
		using (logger.BeginScope(new Dictionary<string, object>
		{
			["sourceClass"] = sourceClass,
			["sourceMethod"] = sourceMethod
		}))
		{
			switch (logLevel)
			{
				case LogLevel.Error:
					logger.LogError("{Message}", message);
					break;
				case LogLevel.Warning:
					logger.LogWarning("{Message}", message);
					break;
				case LogLevel.Information:
					logger.LogInformation("{Message}", message);
					break;
				default: //LogLevel.Debug:
					logger.LogDebug("{Message}", message);
					break;
			}
		}
	}
}
