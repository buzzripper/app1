using App1.System.Servers;
using App1.Auth.Api.Extensions;
using App1.System.Apis.Extensions;
using App1.System.Shared.Context;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (telemetry, health checks, resilience)
builder.AddServiceDefaults();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddRequestContext();
builder.Services.AddStandardJwtAuthentication(builder.Configuration);
builder.Services.AddAuthApiServices(false);
builder.Services.AddStandardApiVersioning();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseStandardApiPipeline();

// Enable API documentation in development
if (app.Environment.IsDevelopment())
{
	app.MapAuthApiDocumentation();
}

app.MapStandardApiEndpoints();
app.MapDefaultEndpoints();

app.Run();
