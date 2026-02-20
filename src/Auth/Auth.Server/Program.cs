using Dyvenix.App1.Auth.Api.Extensions;
using Dyvenix.App1.Common.Api.Extensions;
using Dyvenix.App1.Common.Data.Config;
using Dyvenix.App1.Common.Server;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (telemetry, health checks, resilience)
builder.AddServiceDefaults();

var dataConfig = DataConfigBuilder.Build(builder.Configuration);

// Add services to the container
if (builder.Environment.IsEnvironment("Testing"))
    builder.Services.AddTestJwtAuthentication();
else
    builder.Services.AddStandardJwtAuthentication(builder.Configuration);

builder.Services.AddPermissionAuthorization();
builder.Services.AddAuthApiServices(false);
builder.Services.AddStandardApiVersioning();
builder.Services.AddDataServices(dataConfig);

//----------------------------------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseStandardApiPipeline();

app.MapEndpoints();
app.MapDefaultEndpoints();

// Enable API documentation in development
if (app.Environment.IsDevelopment())
    app.MapAuthApiDocumentation();

app.Run();
