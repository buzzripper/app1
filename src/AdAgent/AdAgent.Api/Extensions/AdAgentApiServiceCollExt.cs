using Dyvenix.App1.AdAgent.Api.Endpoints;
using Dyvenix.App1.App.Api.Services;
using Dyvenix.App1.Common.Shared.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Dyvenix.App1.AdAgent.Api.Extensions;

public static partial class AdAgentApiServiceCollExt
{
    // Declaration of partial methods for code-generated services
    public static partial void AddGeneratedServices(IServiceCollection services);
    public static partial void MapGeneratedEndpoints(IEndpointRouteBuilder app);

    /// <summary>
    /// Registers App API services.
    /// Call this when hosting App services (standalone or in-process).
    /// </summary>
    public static IServiceCollection AddAdAgentApiServices(this IServiceCollection services, bool isInProcess)
    {
        // Register business logic services
        services.AddScoped<ISystemService, AdAgentSystemService>();
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
        app.MapAdAgentSystemEndpoints();

        MapGeneratedEndpoints(app);

        return app;
    }

    /// <summary> 
    /// Maps OpenAPI and Scalar API documentation endpoints for Auth API.
    /// Call this in development or when you want to expose API documentation.
    /// </summary>
    public static IEndpointRouteBuilder MapAdAgentApiDocumentation(this IEndpointRouteBuilder app)
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
