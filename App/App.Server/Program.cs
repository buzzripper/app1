using App1.App.Api.Extensions;
using App1.App.Shared.Authorization;
using App1.System.Servers;
using App1.System.Apis.Extensions;
using App1.System.Shared.Authorization;
using App1.System.Shared.Context;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (telemetry, health checks, resilience)
builder.AddServiceDefaults();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddRequestContext();
builder.Services.AddPermissionAuthorization();
builder.Services.AddAppAuthorization();

// Use Entra ID JWT authentication (validates tokens from Portal or direct API calls)
builder.Services.AddEntraIdJwtAuthentication(builder.Configuration);

builder.Services.AddAppApiServices(false);
builder.Services.AddStandardApiVersioning();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseStandardApiPipeline();

// Enable API documentation in development
if (app.Environment.IsDevelopment())
{
	app.MapAppApiDocumentation();
}

app.MapStandardApiEndpoints();
app.MapDefaultEndpoints();

app.Run();
