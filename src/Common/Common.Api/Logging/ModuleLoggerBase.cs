using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Dyvenix.App1.Common.Api.Logging;

/// <summary>
/// Thin wrapper around ILogger that includes a module name in the scope.
/// </summary>
public interface IModuleLogger
{
	void Error(Exception ex, [CallerFilePath] string sourceClass = "", [CallerMemberName] string sourceMethod = "");
	void Warn(string message, [CallerFilePath] string sourceClass = "", [CallerMemberName] string sourceMethod = "");
	void Info(string message, [CallerFilePath] string sourceClass = "", [CallerMemberName] string sourceMethod = "");
	void Debug(string message, [CallerFilePath] string sourceClass = "", [CallerMemberName] string sourceMethod = "");
}

public abstract class ModuleLoggerBase
{
	private readonly ILogger _logger;
	private readonly string _module;

	public ModuleLoggerBase(ILoggerFactory loggerFactory, string module)
	{
		_module = module;
		_logger = loggerFactory.CreateLogger(module);
	}

	public void Error(Exception ex, [CallerFilePath] string sourceFile = "", [CallerMemberName] string sourceMethod = "")
	{
		Log(LogLevel.Error, ex.GetBaseException().Message, ParseSourceClass(sourceFile), sourceMethod);
	}

	public void Warn(string message, [CallerFilePath] string sourceFile = "", [CallerMemberName] string sourceMethod = "")
	{
		Log(LogLevel.Warning, message, ParseSourceClass(sourceFile), sourceMethod);
	}

	public void Info(string message, [CallerFilePath] string sourceFile = "", [CallerMemberName] string sourceMethod = "")
	{
		Log(LogLevel.Information, message, ParseSourceClass(sourceFile), sourceMethod);
	}

	public void Debug(string message, [CallerFilePath] string sourceFile = "", [CallerMemberName] string sourceMethod = "")
	{
		Log(LogLevel.Debug, message, ParseSourceClass(sourceFile), sourceMethod);
	}

	private static string ParseSourceClass(string sourceFile)
	{
		if (sourceFile.Contains('.'))
			return Path.GetFileNameWithoutExtension(sourceFile);
		return sourceFile;
	}

	private void Log(LogLevel logLevel, string message, string sourceClass, string sourceMethod)
	{
		using (_logger.BeginScope(new Dictionary<string, object>
		{
			["sourceClass"] = sourceClass,
			["sourceMethod"] = sourceMethod,
			["module"] = _module
		}))
		{
			switch (logLevel)
			{
				case LogLevel.Error:
					_logger.LogError("{Message}", message);
					break;
				case LogLevel.Warning:
					_logger.LogWarning("{Message}", message);
					break;
				case LogLevel.Information:
					_logger.LogInformation("{Message}", message);
					break;
				default:
					_logger.LogDebug("{Message}", message);
					break;
			}
		}
	}
}