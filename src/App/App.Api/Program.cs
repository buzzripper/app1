using Dyvenix.App1.App.Api.Extensions;
using Dyvenix.App1.App.Shared.Authorization;
using Dyvenix.App1.Common.Api.Authorization;
using Dyvenix.App1.Common.Api.Extensions.BuilderExtensions;
using Dyvenix.App1.Common.Api.Extensions.SvcCollExtensions;
using Dyvenix.App1.Common.Api.Extensions.WebAppExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi.Generated;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.ConfigureOpenTelemetry();

// Add .NET services
services.AddServiceDiscovery();
services.AddOpenApi();

// Add Common services
services.AddDefaultHealthChecks();
services.AddStandardApiVersioning();
services.AddPermissionAuthorization();
if (builder.Environment.IsEnvironment("Testing"))
	services.AddTestJwtAuthentication();
else
	services.AddJwtBearerAuthentication(builder.Configuration);

// Add App.API services
services.AddAppApiServices(builder.Configuration);


//----------------------------------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Common middleware
app.MapHealthEndpoints();

// Register permissions for this service
app.Services.GetRequiredService<PermissionRegistry>()
	.Register(AppPermissions.Hierarchy);

// Map endpoints for this service
app.MapAppEndpoints();

// Enable API documentation in development
if (app.Environment.IsDevelopment())
	app.MapAppApiDocumentation();


app.Run();
