using System;

namespace Dyvenix.System.Shared.DTOs;

public class HealthStatus
{
	public bool IsHealthy { get; set; }
	public string Message { get; set; } = "Service is running";
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
