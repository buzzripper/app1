using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace Dyvenix.App1.Portal.Server;

public class DynamicProxyConfigProvider : IProxyConfigProvider
{
	private readonly IConfiguration _configuration;
	private volatile DynamicProxyConfig _config;

	public DynamicProxyConfigProvider(IConfiguration configuration)
	{
		_configuration = configuration;
		_config = new DynamicProxyConfig(LoadRoutes(), LoadClusters());
	}

	public IProxyConfig GetConfig() => _config;

	private IReadOnlyList<RouteConfig> LoadRoutes()
	{
		var routes = new List<RouteConfig>();
		
		// Load all routes from appsettings.json
		var configRoutes = _configuration.GetSection("ReverseProxy:Routes")
			.GetChildren()
			.ToList();

		foreach (var routeSection in configRoutes)
		{
			var routeId = routeSection.Key;

			// Skip auth-api route if Auth is running in-process
#if !AUTH_EXT
			if (routeId == "auth-api")
			{
				Console.WriteLine($"[YARP] Skipping route '{routeId}' - Auth running in-process");
				continue;
			}
#endif

			// Skip app1-api route if App1 is running in-process
#if !APP1_EXT
			if (routeId == "app1-api")
			{
				Console.WriteLine($"[YARP] Skipping route '{routeId}' - App1 running in-process");
				continue;
			}
#endif

			Console.WriteLine($"[YARP] Loading route '{routeId}'");

			var route = new RouteConfig
			{
				RouteId = routeId,
				ClusterId = routeSection["ClusterId"],
				Match = new RouteMatch
				{
					Path = routeSection["Match:Path"]
				}
			};

			// Add transforms if they exist
			var transforms = routeSection.GetSection("Transforms").GetChildren().ToList();
			if (transforms.Any())
			{
				var transformsList = new List<IReadOnlyDictionary<string, string>>();
				foreach (var transform in transforms)
				{
					var dict = transform.GetChildren()
						.ToDictionary(x => x.Key, x => x.Value ?? string.Empty);
					transformsList.Add(dict);
				}
				route = route with { Transforms = transformsList };
			}

			routes.Add(route);
		}

		Console.WriteLine($"[YARP] Total routes loaded: {routes.Count}");
		return routes;
	}

	private IReadOnlyList<ClusterConfig> LoadClusters()
	{
		var clusters = new List<ClusterConfig>();
		
		// Load all clusters from appsettings.json
		var configClusters = _configuration.GetSection("ReverseProxy:Clusters")
			.GetChildren()
			.ToList();

		foreach (var clusterSection in configClusters)
		{
			var clusterId = clusterSection.Key;

			// Skip auth-cluster if Auth is running in-process
#if !AUTH_EXT
			if (clusterId == "auth-cluster")
				continue;
#endif

			// Skip app1-cluster if App1 is running in-process
#if !APP1_EXT
			if (clusterId == "app1-cluster")
				continue;
#endif

			var destinations = new Dictionary<string, DestinationConfig>();
			var destinationsSection = clusterSection.GetSection("Destinations").GetChildren();
			
			foreach (var destSection in destinationsSection)
			{
				destinations[destSection.Key] = new DestinationConfig
				{
					Address = destSection["Address"] ?? string.Empty
				};
			}

			var cluster = new ClusterConfig
			{
				ClusterId = clusterId,
				Destinations = destinations
			};

			// Add HttpClient config if it exists
			var httpClientSection = clusterSection.GetSection("HttpClient");
			if (httpClientSection.Exists())
			{
				var sslProtocols = httpClientSection.GetSection("SslProtocols")
					.Get<string[]>();
				
				if (sslProtocols?.Any() == true)
				{
					// Combine SSL protocols using bitwise OR
					var combinedProtocols = sslProtocols
						.Select(p => Enum.Parse<global::System.Security.Authentication.SslProtocols>(p))
						.Aggregate((a, b) => a | b);
					
					cluster = cluster with
					{
						HttpClient = new HttpClientConfig
						{
							SslProtocols = combinedProtocols
						}
					};
				}
			}

			clusters.Add(cluster);
		}

		return clusters;
	}

	private class DynamicProxyConfig : IProxyConfig
	{
		private readonly CancellationTokenSource _cts = new();

		public DynamicProxyConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
		{
			Routes = routes;
			Clusters = clusters;
			ChangeToken = new CancellationChangeToken(_cts.Token);
		}

		public IReadOnlyList<RouteConfig> Routes { get; }
		public IReadOnlyList<ClusterConfig> Clusters { get; }
		public IChangeToken ChangeToken { get; }
	}
}
