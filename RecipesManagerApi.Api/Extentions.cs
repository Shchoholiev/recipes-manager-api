using Microsoft.Extensions.Diagnostics.HealthChecks;
using RecipesManagerApi.Api.CustomMiddlewares;
using RecipesManagerApi.Infrastructure.BackgroundServices;
using System.Runtime.CompilerServices;

namespace RecipesManagerApi.Api;

public static class Extentions
{
    public static IApplicationBuilder ConfogureGlobalUserMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalUserCustomMiddleware>();
        return app;
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("allowAnyOrigin",
            builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddMongoDb(
                mongodbConnectionString: configuration.GetConnectionString("MongoDb"),
                name: configuration.GetConnectionString("MongoDatabaseName"),
                failureStatus: HealthStatus.Unhealthy,
                tags: new string[] { "db", "data" },
                timeout: TimeSpan.FromSeconds(30));
        
        services.AddHostedService<HealthCheckBackgroundService>();
        
        return services;
    }
}
