namespace Dyvenix.App1.Common.Shared.Config;

public class ApiClientsConfig : Dictionary<string, ApiClientConfig>
{
}

public class ApiClientConfig
{
	public string BaseUrl { get; set; }
	public int TimeoutSecs { get; set; }
}
