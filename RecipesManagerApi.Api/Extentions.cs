using RecipesManagerApi.Api.CustomMiddlewares;
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
}
