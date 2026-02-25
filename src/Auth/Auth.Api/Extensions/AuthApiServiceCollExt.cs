using Dyvenix.App1.Auth.Api.Config;
using Dyvenix.App1.Auth.Api.Endpoints;
using Dyvenix.App1.Auth.Api.Repositories;
using Dyvenix.App1.Auth.Api.Services;
using Dyvenix.App1.Auth.Shared.Contracts;
using Dyvenix.App1.Common.Shared.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Dyvenix.App1.Auth.Api.Extensions;

public static partial class AuthApiServiceCollExt
{
    // Declaration of partial method for code-generated services
    public static partial void AddGeneratedServices(IServiceCollection services);
    public static partial void MapGeneratedEndpoints(IEndpointRouteBuilder app);

    /// <summary>
    /// Registers Auth API services.
    /// Call this when hosting Auth services (standalone or in-process).
    /// </summary>
    public static IServiceCollection AddAuthApiServices(this IServiceCollection services, bool isInProcess)
    {
        // Register business logic services
        services.AddScoped<ISystemService, AuthSystemService>();
        services.AddScoped<BrandImgService>();

        if (!isInProcess)
        {
            // Add OpenAPI support
            services.AddOpenApi();
        }

        // Add code-generated services
        AddGeneratedServices(services);

        return services;
    }

    /// <summary> 
    /// Maps endpoints
    /// </summary>
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAuthSystemEndpoints();
        app.MapBrandImgEndpoints();

        MapGeneratedEndpoints(app);

        return app;
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

    /// <summary>
    /// Registers the brand image repository based on configuration.
    /// Call after AddAuthApiServices.
    /// </summary>
    public static IServiceCollection AddBrandImgRepository(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BrandImgOptions>(configuration.GetSection("BrandImg"));

        var provider = configuration.GetValue<string>("BrandImg:Provider") ?? "File";
        if (provider.Equals("AzureBlob", StringComparison.OrdinalIgnoreCase))
            services.AddScoped<IBrandImgRepository, AzureBlobBrandImgRepository>();
        else
            services.AddScoped<IBrandImgRepository, FileBrandImgRepository>();

        return services;
    }
}
