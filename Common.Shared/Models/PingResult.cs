namespace Dyvenix.App1.Common.Shared.Models;

public class PingResult
{
	public PingResult()
	{
	}

	public PingResult(string module, string service)
	{
		Module = module;
		Service = service;
	}

	public string Module { get; set; }
	public string Service { get; set; }
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
