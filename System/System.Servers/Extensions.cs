using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace App1.System.Servers;

/// <summary>
/// Common extensions for API server hosts.
/// </summary>
public static class Extensions
{
	/// <summary>
	/// Configures JWT Bearer authentication using settings from the "JwtSettings" configuration section.
	/// </summary>
	public static IServiceCollection AddStandardJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		var jwtSettings = configuration.GetSection("JwtSettings");
		var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is required");

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
