using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Dyvenix.System.Apis;

// Adds common Aspire services: service discovery, resilience, health checks, and OpenTelemetry.
// This project should be referenced by each service project in your solution.
// To learn more about using this project, see https://aka.ms/dotnet/aspire/service-defaults
public static class Extensions
{
	private const string HealthEndpointPath = "/health";
	private const string AlivenessEndpointPath = "/alive";

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

		// Uncomment the following to restrict the allowed schemes for service discovery.
		// builder.Services.Configure<ServiceDiscoveryOptions>(options =>
		// {
		//     options.AllowedSchemes = ["https"];
		// });

		return builder;
	}

	public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
	{
		var loggingConfig = builder.Configuration.GetSection("Logging:File");
		var useFileLogging = loggingConfig.GetValue<bool>("Enabled");
		var logFilePath = loggingConfig.GetValue<string>("Path") ?? "logs/app.log";

		// Configure file logging if enabled (for local development)
		if (useFileLogging)
		{
			// Ensure log directory exists
			var logDir = Path.GetDirectoryName(logFilePath);
			if (!string.IsNullOrEmpty(logDir))
				Directory.CreateDirectory(logDir);

			// Add custom file logger with simple format
			builder.Logging.AddProvider(new FileLoggerProvider(logFilePath));
		}

		// Configure console logging with custom formatter in Development (optional fallback)
		if (builder.Environment.IsDevelopment() && !useFileLogging)
		{
			builder.Logging.AddConsoleFormatter<SimpleLogFormatter, SimpleLogFormatterOptions>();
			builder.Logging.AddConsole(options => options.FormatterName = SimpleLogFormatter.FormatterName);
		}

		builder.Logging.AddOpenTelemetry(logging =>
		{
			logging.IncludeFormattedMessage = true;
			logging.IncludeScopes = true;

			// Export logs to OTLP (Grafana Cloud) - only when configured
			var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
			if (!string.IsNullOrEmpty(otlpEndpoint))
			{
				logging.AddOtlpExporter();
			}
		});

		builder.Services.AddOpenTelemetry()
			.WithMetrics(metrics =>
			{
				metrics.AddAspNetCoreInstrumentation();
				metrics.AddHttpClientInstrumentation();
				metrics.AddRuntimeInstrumentation();
				
				var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
				if (!string.IsNullOrEmpty(otlpEndpoint))
				{
					metrics.AddOtlpExporter();
				}
			})
			.WithTracing(tracing =>
			{
				tracing.AddSource(builder.Environment.ApplicationName)
					.AddAspNetCoreInstrumentation(tracingOptions =>
						// Exclude health check requests from tracing
						tracingOptions.Filter = context =>
							!context.Request.Path.StartsWithSegments(HealthEndpointPath)
							&& !context.Request.Path.StartsWithSegments(AlivenessEndpointPath)
					)
					.AddHttpClientInstrumentation();
				
				var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
				if (!string.IsNullOrEmpty(otlpEndpoint))
				{
					tracing.AddOtlpExporter();
				}
			});

		builder.AddOpenTelemetryExporters();

		return builder;
	}

	private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
	{
		// OTLP exporter is already configured via AddOtlpExporter() calls in ConfigureOpenTelemetry
		// This method is intentionally empty but kept as a placeholder for future extensibility
		// (e.g., Azure Monitor or other observability backends)
		
		// Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
		//if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
		//{
		//    builder.Services.AddOpenTelemetry()
		//       .UseAzureMonitor();
		//}

		return builder;
	}

	public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
	{
		builder.Services.AddHealthChecks()
			// Add a default liveness check to ensure app is responsive
			.AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

		return builder;
	}

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
