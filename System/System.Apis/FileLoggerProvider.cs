using Microsoft.Extensions.Logging;

namespace Dyvenix.System.Apis;

public sealed class FileLoggerProvider(string filePath, string serviceName) : ILoggerProvider
{
	private readonly string _filePath = filePath;
	private readonly string _serviceName = serviceName;
	private readonly object _lock = new();

	public ILogger CreateLogger(string categoryName) => new FileLogger(categoryName, _filePath, _serviceName, _lock);

	public void Dispose() { }
}

internal sealed class FileLogger(string categoryName, string filePath, string serviceName, object fileLock) : ILogger
{
	private readonly string _categoryName = categoryName;
	private readonly string _filePath = filePath;
	private readonly string _serviceName = serviceName;
	private readonly object _fileLock = fileLock;

	private static readonly AsyncLocal<Dictionary<string, object>?> _currentScope = new();

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull
	{
		if (state is Dictionary<string, object> dict)
		{
			_currentScope.Value = dict;
			return new ScopeDisposable();
		}
		return null;
	}

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

		var sourceClass = _currentScope.Value?.GetValueOrDefault("sourceClass")?.ToString() ?? "";
		var sourceMethod = _currentScope.Value?.GetValueOrDefault("sourceMethod")?.ToString() ?? "";

		var logLine = $"{DateTime.Now:HH:mm:ss.fff}\t{GetLogLevelAbbreviation(logLevel)}\t{_serviceName}\t{sourceClass}\t{sourceMethod}\t{message}";

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

	private class ScopeDisposable : IDisposable
	{
		public void Dispose() => _currentScope.Value = null;
	}
}
