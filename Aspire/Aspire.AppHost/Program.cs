var builder = DistributedApplication.CreateBuilder(args);

// Auth microservice
var authServer = builder.AddProject<Projects.Auth_Server>("auth-server");
//.WithHttpsEndpoint(port: 5002, name: "https");

// App microservice  
var appServer = builder.AddProject<Projects.App_Server>("app-server");
//.WithHttpsEndpoint(port: 5003, name: "https");

// Portal BFF - references Auth and App for service discovery
var portalServer = builder.AddProject<Projects.Portal_Server>("portal-server")
	//.WithHttpsEndpoint(port: 5001, name: "https")
	.WithReference(authServer)
	.WithReference(appServer);

builder.Build().Run();
