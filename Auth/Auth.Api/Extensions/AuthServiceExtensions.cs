using App1.App.Api.Filters;
using App1.App.Api.Logging;
using App1.Auth.Api.Services;
using App1.Auth.Shared.Interfaces;
using App1.System.Apis.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scalar.AspNetCore;

namespace App1.Auth.Api.Extensions;

public static class AuthServiceExtensions
{
	/// <summary>
	/// Registers Auth API services and controllers.
	/// Call this when hosting Auth services (standalone or in-process).
	/// </summary>
	public static IServiceCollection AddAuthApiServices(this IServiceCollection services, bool isInProcess)
	{
		// Register business logic services
		services.AddScoped<IAuthSystemService, AuthSystemService>();
		services.AddTransient<IAuthModuleLogger>(sp => new AuthModuleLogger(sp.GetRequiredService<ILoggerFactory>()));
		
		// Register exception filter for ServiceFilter attribute
		services.AddScoped<AuthExceptionFilter<AuthSystemService>>();

		if (!isInProcess)
		{
			// Add Controllers
			services.AddControllers();

			// Add OpenAPI support
			services.AddOpenApi();
		}

		return services;
	}

	/// <summary>
	/// Maps OpenAPI and Scalar API documentation endpoints for Auth API.
	/// Call this in development or when you want to expose API documentation.
	/// </summary>
	public static IEndpointRouteBuilder MapAuthApiDocumentation(this IEndpointRouteBuilder app)
	{
		app.MapOpenApi();
		app.MapScalarApiReference(options =>
		{
			options
				.WithTitle("Auth API")
				.WithTheme(ScalarTheme.DeepSpace);
		});

		return app;
	}
}
