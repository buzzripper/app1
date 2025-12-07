using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App1.Functions.Functions;

public class SystemFunctions
{
    private const string ModuleId = "App1.Functions";
    private readonly ILogger<SystemFunctions> _logger;

    public SystemFunctions(ILogger<SystemFunctions> logger)
    {
        _logger = logger;
    }

    [Function("Ping")]
    public IActionResult Ping(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "system/Ping")] HttpRequest req)
    {
        return new OkObjectResult(new PingResult(ModuleId, nameof(SystemFunctions)));
    }
}

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

    public string Module { get; set; } = string.Empty;
    public string Service { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
