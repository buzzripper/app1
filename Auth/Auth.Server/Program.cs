using Dyvenix.System.Apis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Dyvenix.Auth.Api.Extensions;

const string AzureTenantId = "1c3cdcca-ba60-4ad2-9892-626f5d92bc09";

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (telemetry, health checks, etc.)
builder.AddServiceDefaults();

// Add services to the container
builder.Services.AddControllers();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is required");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtSettings["Issuer"],
			ValidAudience = jwtSettings["Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
		};
	})
	.AddJwtBearer("EntraIdTokenEnrichment", options =>
    {
        options.Authority = $"https://login.microsoftonline.com/{AzureTenantId}/v2.0";
        options.Audience = "{Your API App ID URI}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://login.microsoftonline.com/{AzureTenantId}/v2.0",
            ValidateAudience = true,
            ValidAudience = "{Your API App ID URI}",
            ValidateLifetime = true
        };
	});

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EntraIdTokenEnrichment", policy =>
    {
        policy.AuthenticationSchemes.Add("EntraIdTokenEnrichment");
        policy.RequireAuthenticatedUser();
        // Optionally validate the caller is Microsoft's token service
        policy.RequireClaim("azp", "99045fe1-7639-4a75-9d4a-577b6ca3810f");
    });
});

// Register Auth API services
builder.Services.AddAuthApiServices(false);

// Configure API versioning
builder.Services.AddApiVersioning(options =>
{
	options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Enable Scalar API documentation in development
if (app.Environment.IsDevelopment())
{
	app.MapAuthApiDocumentation();
}

app.MapDefaultEndpoints();

app.Run();
