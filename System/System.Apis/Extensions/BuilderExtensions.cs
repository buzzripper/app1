using Dyvenix.System.Apis.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace Dyvenix.System.Apis.Extensions;

/// <summary>
/// Common extensions for resilience, health checks, and OpenTelemetry.
/// This project should be referenced by each service project in your solution.
/// </summary>
public static class BuilderExtensions
{
	private const string HealthEndpointPath = "/health";
	private const string AlivenessEndpointPath = "/alive";

	/// <summary>
	/// Adds service defaults including OpenTelemetry, health checks, and HTTP client resilience.
	/// </summary>
	public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
	{
		builder.ConfigureOpenTelemetry();

		builder.AddDefaultHealthChecks();

		builder.Services.ConfigureHttpClientDefaults(http =>
		{
			// Turn on resilience by default
			http.AddStandardResilienceHandler();
		});

		return builder;
	}

	/// <summary>
	/// Configures OpenTelemetry for logging with OTLP export.
	/// </summary>
	public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
	{
		// Get OTEL configuration from appsettings
		var serviceName = builder.Configuration["OTEL_SERVICE_NAME"] ?? builder.Environment.ApplicationName;

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
			builder.Logging.AddProvider(new FileLoggerProvider(logFilePath, serviceName));
		}

		// Configure console logging with custom formatter in Development (optional fallback)
		if (builder.Environment.IsDevelopment() && !useFileLogging)
		{
			builder.Logging.AddConsoleFormatter<SimpleLogFormatter, SimpleLogFormatterOptions>();
			builder.Logging.AddConsole(options => options.FormatterName = SimpleLogFormatter.FormatterName);
		}

		// Get remaining OTEL configuration
		var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
		var otlpHeaders = builder.Configuration["OTEL_EXPORTER_OTLP_HEADERS"];
		var otlpProtocol = builder.Configuration["OTEL_EXPORTER_OTLP_PROTOCOL"];
		var resourceAttributes = builder.Configuration["OTEL_RESOURCE_ATTRIBUTES"];

		builder.Logging.AddOpenTelemetry(logging =>
		{
			logging.IncludeFormattedMessage = true;
			logging.IncludeScopes = true;

			// Configure resource attributes (service name, environment, etc.)
			var resourceBuilder = ResourceBuilder.CreateDefault()
				.AddService(serviceName);

			// Parse and add custom resource attributes (e.g., "deployment.environment=dev")
			if (!string.IsNullOrEmpty(resourceAttributes))
			{
				var attributes = resourceAttributes
					.Split(',')
					.Select(attr => attr.Split('='))
					.Where(parts => parts.Length == 2)
					.Select(parts => new KeyValuePair<string, object>(parts[0].Trim(), parts[1].Trim()));

				resourceBuilder.AddAttributes(attributes);
			}

			logging.SetResourceBuilder(resourceBuilder);

			// Export logs to OTLP (Grafana Cloud) - only when configured
			if (!string.IsNullOrEmpty(otlpEndpoint))
			{
				logging.AddOtlpExporter(options =>
				{
					options.Endpoint = new Uri(otlpEndpoint);

					if (!string.IsNullOrEmpty(otlpHeaders))
					{
						options.Headers = otlpHeaders;
					}

					// Parse protocol (default to HttpProtobuf)
					options.Protocol = otlpProtocol?.ToLowerInvariant() switch
					{
						"grpc" => OtlpExportProtocol.Grpc,
						"http/protobuf" => OtlpExportProtocol.HttpProtobuf,
						_ => OtlpExportProtocol.HttpProtobuf
					};
				});
			}
		});

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
	/// Maps health check endpoints for readiness and liveness probes (development only).
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

	/// <summary>
	/// Adds controllers with the global exception filter.
	/// </summary>
	public static IServiceCollection AddControllersWithExceptionHandling(this IServiceCollection services)
	{
		services.AddControllers(options =>
		{
			options.Filters.Add<GlobalExceptionFilter>();
		});

		return services;
	}

	/// <summary>
	/// Adds controllers with views and the global exception filter.
	/// </summary>
	public static IMvcBuilder AddControllersWithViewsAndExceptionHandling(this IServiceCollection services)
	{
		return services.AddControllersWithViews(options =>
		{
			options.Filters.Add<GlobalExceptionFilter>();
		});
	}
}
