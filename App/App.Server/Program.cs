using App1.App.Api.Extensions;
using Dyvenix.App1.Common.Api.Extensions;
using Dyvenix.App1.Common.Server;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (telemetry, health checks, resilience)
builder.AddServiceDefaults();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddStandardJwtAuthentication(builder.Configuration);
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
