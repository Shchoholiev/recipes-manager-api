using Microsoft.Extensions.DependencyInjection;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Infrastructure.Database;
using RecipesManagerApi.Infrastructure.Repositories;
using AutoMapper;
using RecipesManagerApi.Application.MappingProfiles;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Infrastructure.Services;

namespace RecipesManagerApi.Infrastructure;

public static class MiddlewareExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<MongoDbContext>();
        
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IRolesRepository, RolesRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IRolesService, RolesService>();
        services.AddScoped<IUsersService, UsersService>();

        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CategoryProfile));
        services.AddAutoMapper(typeof(UserProfile));
        services.AddAutoMapper(typeof(RoleProfile));

        return services;
    }
}
