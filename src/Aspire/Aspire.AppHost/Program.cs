var builder = DistributedApplication.CreateBuilder(args);

// Auth microservice
var authServer = builder.AddProject<Projects.Auth_Server>("auth-server");

// App microservice  
//var appServer = builder.AddProject<Projects.App_Server>("app-server")
//	.WithReference(authServer);

// Integration microservice
var integrationServer = builder.AddProject<Projects.Integration_Server>("integration-server");

// AdAgent microservice
var adAgentServer = builder.AddProject<Projects.AdAgent_Server>("adagent-server");

// Portal BFF - references Auth, App, and AdAgent for service discovery
var portalServer = builder.AddProject<Projects.Portal_Server>("portal-server")
	.WithReference(authServer)
	//.WithReference(appServer)
	.WithReference(adAgentServer)
	.WithReference(integrationServer);

try
{
	await builder.Build().RunAsync();
}
catch (OperationCanceledException)
{
	// Expected when Aspire.Hosting.Testing tears down the DistributedApplicationFactory.
}
