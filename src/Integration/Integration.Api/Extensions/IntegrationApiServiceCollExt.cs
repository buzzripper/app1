using Dyvenix.App1.App.Api.Services.v1;
using Dyvenix.App1.App.Endpoints.v1;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.Common.Shared.Contracts;
using Dyvenix.App1.Integration.Api.Endpoints;
using Dyvenix.App1.Integration.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Dyvenix.App1.Integration.Api.Extensions;

public static partial class IntegrationApiServiceCollExt
{
    // Declaration of partial methods for code-generated services
    static partial void AddGeneratedServices(IServiceCollection services);
    static partial void MapGeneratedEndpoints(IEndpointRouteBuilder app);

    /// <summary>
    /// Registers Integration API services.
    /// Call this when hosting Integration services (standalone or in-process).
    /// </summary>
    public static IServiceCollection AddIntegrationApiServices(this IServiceCollection services, bool isInProcess)
    {
        // Register business logic services
        services.AddScoped<ISystemService, IntegrationSystemService>();
        services.AddScoped<IImportService, ImportService>();

        // Add code-generated services
        AddGeneratedServices(services);

        if (!isInProcess)
        {
            // Add OpenAPI support
            services.AddOpenApi();
        }

        return services;
    }

    /// <summary>
    /// Maps endpoints
    /// </summary>
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapIntegrationSystemEndpoints();
        app.MapImportEndpoints();
        MapGeneratedEndpoints(app);

        return app;
    }

    /// <summary>
    /// Maps OpenAPI and Scalar API documentation endpoints for Integration API.
    /// Call this in development or when you want to expose API documentation.
    /// </summary>
    public static IEndpointRouteBuilder MapIntegrationApiDocumentation(this IEndpointRouteBuilder app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Integration API")
                .WithTheme(ScalarTheme.DeepSpace);
        });

        return app;
    }
}
