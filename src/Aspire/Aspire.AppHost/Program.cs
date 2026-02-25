var builder = DistributedApplication.CreateBuilder(args);

// Auth microservice
var authServer = builder.AddProject<Projects.Auth_Server>("auth-server");
//.WithHttpsEndpoint(port: 5002, name: "https");

// App microservice  
var appServer = builder.AddProject<Projects.App_Server>("app-server");
//.WithHttpsEndpoint(port: 5003, name: "https");

// AdAgent microservice
var adAgentServer = builder.AddProject<Projects.AdAgent_Server>("adagent-server");

// Portal BFF - references Auth, App, and AdAgent for service discovery
var portalServer = builder.AddProject<Projects.Portal_Server>("portal-server")
	//.WithHttpsEndpoint(port: 5001, name: "https")
	.WithReference(authServer)
	.WithReference(appServer)
	.WithReference(adAgentServer);

try
{
	await builder.Build().RunAsync();
}
catch (OperationCanceledException)
{
	// Expected when Aspire.Hosting.Testing tears down the DistributedApplicationFactory.
}
