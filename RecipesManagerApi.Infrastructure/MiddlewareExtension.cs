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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
		services.AddScoped<IImagesRepository, ImagesRepository>();
		services.AddScoped<IOpenAiLogsRepository, OpenAiLogsRepository>();
		services.AddScoped<IMenusRepository, MenusRepository>();
		services.AddScoped<ISharedRecipesRepository, SharedRecipeRepository>();
		services.AddScoped<ISavedRecipesRepository, SavedRecipesRepository>();
		services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
		services.AddScoped<IContactsRepository, ContactsRepository>();
		services.AddScoped<IShoppingListsRepository, ShoppingListsRepository>();
		services.AddTransient<ILogsRepository, LogsRepository>();

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
		services.AddScoped<IImagesService, ImagesService>();
		services.AddScoped<IEmailsService, EmailsService>();
		services.AddScoped<IUserManager, UserManager>();
		services.AddScoped<IOpenAiService, OpenAiService>();
		services.AddScoped<ISharedRecipesService, SharedRecipesService>();
		services.AddScoped<IIngredientsService, IngredientsService>();
		services.AddScoped<ISavedRecipesService, SavedRecipesService>();
		services.AddScoped<ISubscriptionService, SubscriptionsService>();
		services.AddTransient<ILogsService, LogsService>();
		services.AddScoped<IOpenAiLogsService, OpenAiLogsService>();
		services.AddScoped<IShoppingListsService, ShoppingListsService>();
		services.AddScoped<IMenusService, MenusService>();
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
				.AddTypeExtension<MenusQuery>()
				.AddTypeExtension<SharedRecipesQuery>()
				.AddTypeExtension<SavedRecipesQuery>()
				.AddTypeExtension<RecipesQuery>()
				.AddTypeExtension<UsersQuery>()
				.AddTypeExtension<SubscriptionsQuery>()
				.AddTypeExtension<ShoppingListsQuery>()
				.AddTypeExtension<LogsQuery>()
				.AddTypeExtension<OpenAiLogsQuery>()
                 .AddTypeExtension<RolesQuery>()
            .AddMutationType()
				.AddTypeExtension<SharedRecipesMutation>()
				.AddTypeExtension<CategoriesMutation>()
				.AddTypeExtension<RegisterMutation>()
				.AddTypeExtension<LoginMutation>()
				.AddTypeExtension<AccessMutation>()
				.AddTypeExtension<UserMutation>()
				.AddTypeExtension<RoleMutation>()
				.AddTypeExtension<ContactsMutation>()
				.AddTypeExtension<MenusMutation>()
        .AddTypeExtension<RecipesMutation>()
				.AddTypeExtension<SavedRecipesMutation>()
				.AddTypeExtension<SubscriptionsMutation>()
				.AddTypeExtension<ShoppingListsMutation>()
			.AddAuthorization()
			.InitializeOnStartup(keepWarm: true);

		return services;
	}

	public static IServiceCollection AddJWTTokenAuthentication(this IServiceCollection services,
													 IConfiguration configuration)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		.AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = configuration.GetValue<bool>("JsonWebTokenKeys:ValidateIssuer"),
				ValidateAudience = configuration.GetValue<bool>("JsonWebTokenKeys:ValidateAudience"),
				ValidateLifetime = configuration.GetValue<bool>("JsonWebTokenKeys:ValidateLifetime"),
				ValidateIssuerSigningKey = configuration.GetValue<bool>("JsonWebTokenKeys:ValidateIssuerSigningKey"),
				ValidIssuer = configuration.GetValue<string>("JsonWebTokenKeys:ValidIssuer"),
				ValidAudience = configuration.GetValue<string>("JsonWebTokenKeys:ValidAudience"),
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JsonWebTokenKeys:IssuerSigningKey"))),
				ClockSkew = TimeSpan.Zero
			};
		});

		return services;
	}

	public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
	{
		var openAiApiKey = configuration.GetSection("OpenAi")?.GetValue<string>("ApiKey");
		services.AddHttpClient("OpenAiHttpClient", client => {
			client.BaseAddress = new Uri("https://api.openai.com/v1/");
			client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");
		});

		return services;
	}
}