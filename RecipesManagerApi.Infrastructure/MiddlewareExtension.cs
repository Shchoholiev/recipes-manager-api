using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RecipesManagerApi.Infrastructure.Database;

namespace RecipesManagerApi.Infrastructure;

public static class MiddlewareExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<MongoDbContext>();

        return services;
    }
}
