using System.Text.Json.Serialization;

namespace Dyvenix.App1.Common.Shared.DTOs;

public class ServiceInfo
{
    // Identity
    public string ServiceName { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string HostName { get; set; } = string.Empty;

    // Health
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StatusLevel Status { get; set; }
    public string? StatusMessage { get; set; }

    // Runtime
    public DateTime StartTimeUtc { get; set; }
    public TimeSpan Uptime { get; set; }

    // Service-specific metadata
    public List<ServiceInfoItem> Items { get; set; } = new List<ServiceInfoItem>();
}
