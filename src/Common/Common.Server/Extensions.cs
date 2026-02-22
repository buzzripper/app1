using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

namespace Dyvenix.App1.Common.Server;

/// <summary>
/// Common extensions for API server hosts.
/// </summary>
public static class Extensions
{
	/// <summary>
	/// Configures JWT Bearer authentication using Microsoft Entra ID token validation.
	/// Reads settings from the "MicrosoftEntraID" configuration section.
	/// </summary>
	public static IServiceCollection AddEntraIdApiAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMicrosoftIdentityWebApiAuthentication(configuration, "MicrosoftEntraID");
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
