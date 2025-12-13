using Dyvenix.App.Api.Filters;
using Dyvenix.App.Api.Logging;
using Dyvenix.App.Api.Services;
using Dyvenix.App.Shared.Interfaces;
using Dyvenix.System.Apis.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scalar.AspNetCore;

namespace Dyvenix.App.Api.Extensions;

public static class AppServiceExtensions
{
	/// <summary>
	/// Registers App API services and controllers.
	/// Call this when hosting App services (standalone or in-process).
	/// </summary>
	public static IServiceCollection AddAppApiServices(this IServiceCollection services, bool isInProcess)
	{
		// Register business logic services
		services.AddScoped<IAppSystemService, AppSystemService>();
		services.AddTransient<IAppModuleLogger>(sp => new AppModuleLogger(sp.GetRequiredService<ILoggerFactory>()));

		// Register exception filter for ServiceFilter attribute
		services.AddScoped<AppExceptionFilter<AppSystemService>>();

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
	/// Maps OpenAPI and Scalar API documentation endpoints for App API.
	/// Call this in development or when you want to expose API documentation.
	/// </summary>
	public static IEndpointRouteBuilder MapAppApiDocumentation(this IEndpointRouteBuilder app)
	{
		app.MapOpenApi();
		app.MapScalarApiReference(options =>
		{
			options
				.WithTitle("App API")
				.WithTheme(ScalarTheme.DeepSpace);
		});

		return app;
	}

	///// <summary>
	///// Maps App controllers to the application with optional path prefix.
	///// Call this when hosting App services (standalone or in-process).
	///// </summary>
	///// <param name="app">The endpoint route builder</param>
	///// <param name="pathPrefix">Optional path prefix (e.g., "/api/app"). Use empty string for no prefix.</param>
	//public static IEndpointRouteBuilder MapAppEndpoints(this IEndpointRouteBuilder app, string pathPrefix = "")
	//{
	//	if (string.IsNullOrEmpty(pathPrefix))
	//	{
	//		// No prefix - map controllers directly
	//		app.MapControllers();
	//	}
	//	else
	//	{
	//		// Apply prefix using MapGroup
	//		var group = app.MapGroup(pathPrefix);
	//		group.MapControllers();
	//	}

	//	return app;
	//}
}
