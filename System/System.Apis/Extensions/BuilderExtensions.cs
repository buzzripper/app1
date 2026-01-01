using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace App1.System.Apis.Extensions;

/// <summary>
/// Common extensions for resilience, health checks, and OpenTelemetry.
/// This project should be referenced by each service project in your solution.
/// Based on Aspire ServiceDefaults pattern.
/// </summary>
public static class BuilderExtensions
{
	private const string HealthEndpointPath = "/health";
	private const string AlivenessEndpointPath = "/alive";

	/// <summary>
	/// Adds service defaults including OpenTelemetry, health checks, service discovery, and HTTP client resilience.
	/// </summary>
	public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
	{
		builder.ConfigureOpenTelemetry();

		builder.AddDefaultHealthChecks();

		builder.Services.AddServiceDiscovery();

		builder.Services.ConfigureHttpClientDefaults(http =>
		{
			// Turn on resilience by default
			http.AddStandardResilienceHandler();

			// Turn on service discovery by default
			http.AddServiceDiscovery();
		});

		return builder;
	}

	/// <summary>
	/// Configures OpenTelemetry for logging, tracing, and metrics with OTLP export.
	/// </summary>
	public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
	{
		// Get OTEL configuration from appsettings or environment
		var serviceName = builder.Configuration["OTEL_SERVICE_NAME"] ?? builder.Environment.ApplicationName;

		var loggingConfig = builder.Configuration.GetSection("Logging:File");
		var useFileLogging = loggingConfig.GetValue<bool>("Enabled");
		var logFilePath = loggingConfig.GetValue<string>("Path") ?? "logs/app.log";

		// Configure file logging if enabled (for local development without Aspire)
		if (useFileLogging)
		{
			var logDir = Path.GetDirectoryName(logFilePath);
			if (!string.IsNullOrEmpty(logDir))
				Directory.CreateDirectory(logDir);

			builder.Logging.AddProvider(new FileLoggerProvider(logFilePath, serviceName));
		}

		// Configure console logging with custom formatter in Development (optional fallback)
		if (builder.Environment.IsDevelopment() && !useFileLogging)
		{
			builder.Logging.AddConsoleFormatter<SimpleLogFormatter, SimpleLogFormatterOptions>();
			builder.Logging.AddConsole(options => options.FormatterName = SimpleLogFormatter.FormatterName);
		}

		// Configure OpenTelemetry logging
		builder.Logging.AddOpenTelemetry(logging =>
		{
			logging.IncludeFormattedMessage = true;
			logging.IncludeScopes = true;
		});

		// Configure OpenTelemetry tracing and metrics
		builder.Services.AddOpenTelemetry()
			.WithMetrics(metrics =>
			{
				metrics.AddAspNetCoreInstrumentation()
					.AddHttpClientInstrumentation()
					.AddRuntimeInstrumentation();
			})
			.WithTracing(tracing =>
			{
				tracing.AddAspNetCoreInstrumentation()
					.AddHttpClientInstrumentation();
			});

		// Add OTLP exporter if endpoint is configured (Aspire Dashboard or external collector)
		builder.AddOpenTelemetryExporters();

		return builder;
	}

	private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
	{
		var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

		if (useOtlpExporter)
		{
			builder.Services.AddOpenTelemetry().UseOtlpExporter();
		}

		return builder;
	}

	public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
	{
		builder.Services.AddHealthChecks()
			// Add a default liveness check to ensure app is responsive
			.AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

		return builder;
	}

	/// <summary>
	/// Maps health check endpoints for readiness and liveness probes.
	/// </summary>
	public static WebApplication MapDefaultEndpoints(this WebApplication app)
	{
		// Adding health checks endpoints to applications in non-development environments has security implications.
		// See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
		if (app.Environment.IsDevelopment())
		{
			// All health checks must pass for app to be considered ready to accept traffic after starting
			app.MapHealthChecks(HealthEndpointPath);

			// Only health checks tagged with the "live" tag must pass for app to be considered alive
			app.MapHealthChecks(AlivenessEndpointPath, new HealthCheckOptions
			{
				Predicate = r => r.Tags.Contains("live")
			});
		}

		return app;
	}
}
