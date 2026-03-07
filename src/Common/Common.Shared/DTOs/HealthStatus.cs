namespace Dyvenix.App1.Common.Shared.DTOs;

public class HealthStatus
{
    public StatusLevel Status { get; set; }
    public string Message { get; set; } = "Service is running";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
