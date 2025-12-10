using Microsoft.Extensions.Logging;

namespace Dyvenix.System.Apis;

public sealed class FileLoggerProvider(string filePath) : ILoggerProvider
{
	private readonly string _filePath = filePath;
	private readonly object _lock = new();

	public ILogger CreateLogger(string categoryName) => new FileLogger(categoryName, _filePath, _lock);

	public void Dispose() { }
}

internal sealed class FileLogger(string categoryName, string filePath, object fileLock) : ILogger
{
	private readonly string _categoryName = categoryName;
	private readonly string _filePath = filePath;
	private readonly object _fileLock = fileLock;

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

	public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

	public void Log<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter)
	{
		if (!IsEnabled(logLevel))
			return;

		var message = formatter(state, exception);
		if (string.IsNullOrEmpty(message))
			return;

		var (module, className) = ParseCategory(_categoryName);
		var logLine = $"{DateTime.Now:HH:mm:ss.fff}\t{GetLogLevelAbbreviation(logLevel)}\t{module}\t{className}\t{message}";

		lock (_fileLock)
		{
			File.AppendAllText(_filePath, logLine + Environment.NewLine);

			if (exception is not null)
			{
				File.AppendAllText(_filePath, exception.ToString() + Environment.NewLine);
			}
		}
	}

	private static string GetLogLevelAbbreviation(LogLevel logLevel) => logLevel switch
	{
		LogLevel.Trace => "TRC",
		LogLevel.Debug => "DBG",
		LogLevel.Information => "INF",
		LogLevel.Warning => "WRN",
		LogLevel.Error => "ERR",
		LogLevel.Critical => "CRT",
		_ => "???"
	};

	private static (string Module, string ClassName) ParseCategory(string category)
	{
		var parts = category.Split('.');

		var module = "System";
		var className = parts.Length > 0 ? parts[^1] : category;

		for (var i = 0; i < parts.Length; i++)
		{
			if (parts[i] is "Auth" or "App" or "Portal" or "System")
			{
				module = parts[i];
				break;
			}
		}

		return (module, className);
	}
}
