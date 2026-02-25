
namespace Dyvenix.App1.AdAgent.Api;

public static class ServiceRuntime
{
    public static DateTime StartTimeUtc { get; private set; }
    public static TimeSpan Uptime => DateTime.UtcNow - StartTimeUtc;

    public static void Start()
    {
        StartTimeUtc = DateTime.UtcNow;
    }
}
