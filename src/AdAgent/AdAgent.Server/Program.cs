using Dyvenix.App1.AdAgent.Api.Config;
using Dyvenix.App1.AdAgent.Api.Extensions;
using Dyvenix.App1.Common.Api.Extensions;
using Dyvenix.App1.Common.Server;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (telemetry, health checks, resilience)
builder.AddServiceDefaults();

// Add services to the container
if (builder.Environment.IsEnvironment("Testing"))
    builder.Services.AddTestJwtAuthentication();
else
    builder.Services.AddJwtBearerAuthentication(builder.Configuration);

builder.Services.AddPermissionAuthorization();
builder.Services.AddStandardApiVersioning();

var configRepo = new ConfigRepository();
var adAgentConfig = configRepo.GetConfig();
builder.Services.AddAdAgentApiServices(adAgentConfig);

//----------------------------------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseStandardApiPipeline();

app.MapEndpoints();
app.MapDefaultEndpoints();

// Enable API documentation in development
if (app.Environment.IsDevelopment())
    app.MapAdAgentApiDocumentation();

app.Run();
