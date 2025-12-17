using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace App1.System.Servers;

/// <summary>
/// Common extensions for API server hosts.
/// </summary>
public static class Extensions
{
	/// <summary>
	/// Configures JWT Bearer authentication to validate tokens from Microsoft Entra ID.
	/// Uses settings from the "MicrosoftEntraID" configuration section.
	/// This is the standard authentication for Auth/App API services.
	/// </summary>
	public static IServiceCollection AddEntraIdJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		var entraIdSettings = configuration.GetSection("MicrosoftEntraID");
		var instance = entraIdSettings["Instance"] ?? throw new InvalidOperationException("MicrosoftEntraID:Instance is required");
		var tenantId = entraIdSettings["TenantId"] ?? throw new InvalidOperationException("MicrosoftEntraID:TenantId is required");
		var clientId = entraIdSettings["ClientId"] ?? throw new InvalidOperationException("MicrosoftEntraID:ClientId is required");

		// Build the authority URL for token validation
		// For CIAM: https://tenant.ciamlogin.com/tenantId/v2.0
		// For standard Entra ID: https://login.microsoftonline.com/tenantId/v2.0
		var authority = $"{instance.TrimEnd('/')}/{tenantId}/v2.0";

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.Authority = authority;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidAudience = clientId,
					// Entra ID tokens use the authority as issuer
					ValidIssuer = authority
				};

				// For development, you might want to see more details
				options.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = context =>
					{
						var logger = context.HttpContext.RequestServices
							.GetService<ILogger<JwtBearerEvents>>();
						logger?.LogWarning(context.Exception, "JWT authentication failed");
						return Task.CompletedTask;
					}
				};
			});

		services.AddAuthorization();

		return services;
	}

	/// <summary>
	/// Configures standard API versioning with default version 1.0.
	/// </summary>
	public static IServiceCollection AddStandardApiVersioning(this IServiceCollection services)
	{
		services.AddApiVersioning(options =>
		{
			options.DefaultApiVersion = new ApiVersion(1, 0);
			options.AssumeDefaultVersionWhenUnspecified = true;
			options.ReportApiVersions = true;
		}).AddApiExplorer(options =>
		{
			options.GroupNameFormat = "'v'VVV";
			options.SubstituteApiVersionInUrl = true;
		});

		return services;
	}

	/// <summary>
	/// Configures the standard API middleware pipeline.
	/// </summary>
	public static WebApplication UseStandardApiPipeline(this WebApplication app)
	{
		app.UseHttpsRedirection();
		app.UseAuthentication();
		app.UseAuthorization();

		return app;
	}

	/// <summary>
	/// Maps standard API endpoints (controllers and default endpoints).
	/// </summary>
	public static WebApplication MapStandardApiEndpoints(this WebApplication app)
	{
		app.MapControllers();

		return app;
	}
}
