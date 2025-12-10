using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace Dyvenix.System.Apis;

public sealed class SimpleLogFormatter : ConsoleFormatter
{
	public const string FormatterName = "simple";
	private readonly SimpleLogFormatterOptions _options;

	public SimpleLogFormatter(IOptions<SimpleLogFormatterOptions> options)
		: base(FormatterName)
	{
		_options = options.Value;
	}

	public override void Write<TState>(
		in LogEntry<TState> logEntry,
		IExternalScopeProvider? scopeProvider,
		TextWriter textWriter)
	{
		var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);
		if (message is null)
			return;

		// Timestamp
		textWriter.Write(DateTime.Now.ToString(_options.TimestampFormat));
		textWriter.Write('\t');

		// LogLevel (abbreviated)
		textWriter.Write(GetLogLevelAbbreviation(logEntry.LogLevel));
		textWriter.Write('\t');

		// Module and Class from category (e.g., "Dyvenix.Auth.Api.Services.SystemService")
		var (module, className) = ParseCategory(logEntry.Category);
		textWriter.Write(module);
		textWriter.Write('\t');
		textWriter.Write(className);
		textWriter.Write('\t');

		// Message
		textWriter.WriteLine(message);

		// Exception if present
		if (logEntry.Exception is not null)
		{
			textWriter.WriteLine(logEntry.Exception.ToString());
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
		// Category format: "Dyvenix.Auth.Api.Services.SystemService"
		// Extract module (Auth, App, Portal) and class name
		var parts = category.Split('.');
		
		var module = "System";
		var className = parts.Length > 0 ? parts[^1] : category;

		// Look for known module names
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

public sealed class SimpleLogFormatterOptions : ConsoleFormatterOptions
{
	public string TimestampFormat { get; set; } = "HH:mm:ss.fff";
}
