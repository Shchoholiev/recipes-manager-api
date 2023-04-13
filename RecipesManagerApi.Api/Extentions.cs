using RecipesManagerApi.Api.CustomMiddlewares;
using HotChocolate.AspNetCore.Extensions;
using System.Runtime.CompilerServices;

namespace RecipesManagerApi.Api;

public static class Extentions
{
    public static IApplicationBuilder ConfogureGlobalUserMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalUserCustomMiddleware>();
        return app;
    }

    public static IApplicationBuilder AddExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandler>();
        return app;
    }
}
