using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Dyvenix.System.Apis;

/// <summary>
/// Common extensions for resilience, health checks, and OpenTelemetry.
/// This project should be referenced by each service project in your solution.
/// </summary>
public static class Extensions
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
		builder.Logging.AddOpenTelemetry(logging =>
		{
			logging.IncludeFormattedMessage = true;
			logging.IncludeScopes = true;
			// Export logs to OTLP (Grafana Cloud)
			logging.AddOtlpExporter();
		});

		return builder;
	}

	/// <summary>
	/// Adds default health checks including a liveness check.
	/// </summary>
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
}
