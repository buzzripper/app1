var builder = DistributedApplication.CreateBuilder(args);

// Auth microservice
//builder.AddProject<Projects.Auth_Server>("auth");

// App microservice  
var appServer = builder.AddProject<Projects.App_Api>("app-server");

// Portal BFF - references Auth, App, and AdAgent for service discovery
var portalServer = builder.AddProject<Projects.Portal_Server>("portal-server");
//.WithReference(authServer);

try
{
	await builder.Build().RunAsync();
}
catch (OperationCanceledException)
{
	// Expected when Aspire.Hosting.Testing tears down the DistributedApplicationFactory.
}
