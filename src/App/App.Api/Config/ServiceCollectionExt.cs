using App1.App.Shared.Interfaces;
using Dyvenix.App1.App.Api.Controllers.v1;
using Dyvenix.App1.App.Api.Logging;
using Dyvenix.App1.App.Api.Services;
using Dyvenix.App1.Common.Api.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scalar.AspNetCore;

namespace Dyvenix.App1.App.Api.Config;

public static partial class ServiceCollectionExt
{
	// Declaration of partial method for code-generated services
	static partial void AddGeneratedServices(IServiceCollection services);

	/// <summary>
	/// Registers Auth API services and controllers.
	/// Call this when hosting Auth services (standalone or in-process).
	/// </summary>
	public static IServiceCollection AddAuthApiServices(this IServiceCollection services, bool isInProcess)
	{
		// Register business logic services
		services.AddScoped<IAppSystemService, AppSystemService>();
		services.AddTransient<IAppModuleLogger>(sp => new AppModuleLogger(sp.GetRequiredService<ILoggerFactory>()));

		// Register exception filter for ServiceFilter attribute
		services.AddScoped<ApiExceptionFilter<AppSystemService>>();

		if (!isInProcess)
		{
			// Add Controllers
			services.AddControllers();
			// Add OpenAPI support
			services.AddOpenApi();
		}

		// Add code-generated services
		AddGeneratedServices(services);

		return services;
	}

	/// <summary> 
	/// Maps OpenAPI and Scalar API documentation endpoints for Auth API.
	/// Call this in development or when you want to expose API documentation.
	/// </summary>
	public static IEndpointRouteBuilder MapAppApiDocumentation(this IEndpointRouteBuilder app)
	{
		app.MapPatientEndpoints();

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
