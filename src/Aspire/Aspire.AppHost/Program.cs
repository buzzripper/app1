var builder = DistributedApplication.CreateBuilder(args);

// Auth microservice
//var authServer = builder.AddProject<Projects.Auth_Server>("auth-server");

// App microservice  
//var appServer = builder.AddProject<Projects.App_Server>("app-server")
//	.WithReference(authServer);

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
