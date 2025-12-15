using System;

namespace App1.System.Shared.DTOs;

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
