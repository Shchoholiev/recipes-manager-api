using Microsoft.Extensions.DependencyInjection;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Infrastructure.Database;
using RecipesManagerApi.Infrastructure.Repositories;
using AutoMapper;
using RecipesManagerApi.Application.MappingProfiles;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Infrastructure.Services;
using System.Reflection;
using RecipesManagerApi.Application.IServices.Identity;
using RecipesManagerApi.Application.Interfaces.Identity;
using RecipesManagerApi.Infrastructure.Services.Identity;
using RecipesManagerApi.Infrastructure.Queries;
using RecipesManagerApi.Infrastructure.Mutations;

namespace RecipesManagerApi.Infrastructure;

public static class MiddlewareExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<MongoDbContext>();
        
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IRolesRepository, RolesRepository>();
        services.AddScoped<IRecipesRepository, RecipesRepository>();
        services.AddScoped<IContactsRepository, ContactsRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IRolesService, RolesService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokensService, TokensService>();
        services.AddScoped<ICloudStorageService, CloudStorageService>();
        services.AddScoped<IRecipesService, RecipesService>();
        services.AddScoped<IContactsService, ContactsService>();

        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(CategoryProfile)));

        return services;
    }

    public static IServiceCollection AddGraphQl(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddQueryType()
                .AddTypeExtension<CategoriesQuery>()
                .AddTypeExtension<ContactsQuery>()
            .AddMutationType()
                .AddTypeExtension<CategoriesMutation>()
                .AddTypeExtension<ContactsMutation>()
            .InitializeOnStartup(keepWarm: true);
        

        return services;
    }
}
