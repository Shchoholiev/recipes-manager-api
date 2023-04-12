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
}
