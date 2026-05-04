using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;
using System.Reflection;

namespace Dyvenix.App1.App.Api.Services;

public class HealthService : IHealthCheck
{
	public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
	{
		HealthStatus healthStatus = HealthStatus.Healthy;

		var healthData = new Dictionary<string, object>
		{
			["Environment"] = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
			["Version"] = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0",
			["HostName"] = Environment.MachineName,
			["StartTimeUtc"] = Process.GetCurrentProcess().StartTime.ToUniversalTime().ToString()
		};

		// TODO: Add any custom health checks here and update healthStatus accordingly.

		switch (healthStatus)
		{
			case HealthStatus.Healthy:
				return HealthCheckResult.Healthy(data: healthData);

			case HealthStatus.Degraded:
				return HealthCheckResult.Degraded(data: healthData);

			default:
				return HealthCheckResult.Unhealthy(data: healthData);
		}
	}
}
