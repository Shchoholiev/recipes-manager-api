using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Infrastructure.Repositories;
using RecipesManagerApi.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using RecipesManagerApi.Application.MappingProfiles;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.IServices.Identity;
using RecipesManagerApi.Infrastructure.Services.Identity;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.GlodalInstances;
using RecipesManagerApi.Application.Models;
using AutoMapper;
using RecipesManagerApi.Application.Interfaces.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace RecipesManagerApi.DbInitializer;

public class DbInitializer
{
	public static void FirstInitialization()
	{
		string thumbnailsPath = @"RecipesManagerApi.DbInitializer/Thumbnails";
		
		IConfiguration configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile(@"RecipesManagerApi.Api/appsettings.Development.json")
			.AddJsonFile(@"RecipesManagerApi.Api/appsettings.Production.json")
			.Build();
		
		ILoggerFactory logger = LoggerFactory.Create(builder => 
		{
			builder.AddConsole();
		});
		
		var serviceProvider = new ServiceCollection()
			.AddSingleton(configuration)
			.AddSingleton(logger)
			.AddSingleton(typeof(ILogger<>), typeof(Logger<>))
			.AddSingleton<MongoDbContext>()
			.AddAutoMapper(Assembly.GetAssembly(typeof(CategoryProfile)))
			
			.AddScoped<IRolesService, RolesService>()
			.AddScoped<IUsersService, UsersService>()
			.AddScoped<ICategoriesService, CategoriesService>()
			.AddScoped<IRecipesService, RecipesService>()
			.AddScoped<IImagesService, ImagesService>()
			.AddScoped<ICloudStorageService, CloudStorageService>()
			.AddScoped<ITokensService, TokensService>()
			.AddScoped<ISavedRecipesService, SavedRecipesService>()
			.AddScoped<ISubscriptionService, SubscriptionsService>()
			.AddScoped<IMenusService, MenusService>()
			.AddScoped<IShoppingListsService, ShoppingListsService>()
			.AddScoped<IEmailsService, EmailsService>()
			.AddScoped<IContactsService, ContactsService>()
			
			.AddScoped<IUserManager, UserManager>()
			.AddScoped<IRolesRepository, RolesRepository>()
			.AddScoped<IUsersRepository, UsersRepository>()
			.AddScoped<ICategoriesRepository, CategoriesRepository>()
			.AddScoped<IRecipesRepository, RecipesRepository>()
			.AddScoped<IImagesRepository, ImagesRepository>()
			.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>()
			.AddScoped<ISavedRecipesRepository, SavedRecipesRepository>()
			.AddScoped<IPasswordHasher, PasswordHasher>()
			.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>()
			.AddScoped<IMenusRepository, MenusRepository>()
			.AddScoped<IShoppingListsRepository, ShoppingListsRepository>()
			.AddScoped<IContactsRepository, ContactsRepository>()
			.BuildServiceProvider();
		
		var rolesService = serviceProvider.GetService<IRolesService>();
		var usersRepository = serviceProvider.GetService<IUsersRepository>();
		var passwordHasher = serviceProvider.GetService<IPasswordHasher>();
		var categoriesService = serviceProvider.GetService<ICategoriesService>();
		var recipesService = serviceProvider.GetService<IRecipesService>();
		var mapper = serviceProvider.GetService<IMapper>();
		var tokensService = serviceProvider.GetService<ITokensService>();
		var savedRecipesService = serviceProvider.GetService<ISavedRecipesService>();
		var subscriptionService = serviceProvider.GetService<ISubscriptionService>();
		var menusService = serviceProvider.GetService<IMenusService>();
		var shoppingListsService = serviceProvider.GetService<IShoppingListsService>();
		
		RoleCreateDto roleGuest = new RoleCreateDto
		{
			Name = "Guest"
		};
		RoleCreateDto roleUser = new RoleCreateDto
		{
			Name = "User"
		};
		RoleCreateDto roleAdmin = new RoleCreateDto
		{
			Name = "Admin"
		};
		
		var roleGuestDto = rolesService.AddRoleAsync(roleGuest, CancellationToken.None).Result;
		var roleUserDto =  rolesService.AddRoleAsync(roleUser, CancellationToken.None).Result;
		var roleAdminDto =  rolesService.AddRoleAsync(roleAdmin, CancellationToken.None).Result;
		
		var entitiesRoles = mapper.Map<List<Role>>(new List<RoleDto> { roleGuestDto, roleUserDto });
		var entitiesAdminRoles = mapper.Map<List<Role>>(new List<RoleDto> { roleGuestDto, roleUserDto, roleAdminDto });
		
		User userDmytro = new User
		{
			Name = "Dmytro",
			Email = "dmytro.kamyshenko@nure.ua",
			Roles = entitiesRoles,
			PasswordHash = passwordHasher.Hash("123456Yuiop"),
			RefreshToken = tokensService.GenerateRefreshToken(),
			RefreshTokenExpiryDate = DateTime.Now.AddDays(7),
			CreatedDateUtc = DateTime.UtcNow
		};
		User userVitalii = new User
		{
			Name = "Vitalii",
			Email = "vitalii.krasnorutskyi@nure.ua",
			Roles = entitiesRoles,
			PasswordHash = passwordHasher.Hash("123456Yuiop"),
			RefreshToken = tokensService.GenerateRefreshToken(),
			RefreshTokenExpiryDate = DateTime.Now.AddDays(7),
			CreatedDateUtc = DateTime.UtcNow
		};
		User userYelyzaveta = new User
		{
			Name = "Yelyzaveta",
			Email = "yelyzaveta.boiko1@nure.ua",
			Roles = entitiesRoles,
			PasswordHash = passwordHasher.Hash("123456Yuiop"),
			RefreshToken = tokensService.GenerateRefreshToken(),
			RefreshTokenExpiryDate = DateTime.Now.AddDays(7),
			CreatedDateUtc = DateTime.UtcNow
		};
		User userPolina = new User
		{
			Name = "Polina",
			Email = "polina.pavlenko@nure.ua",
			Roles = entitiesRoles,
			PasswordHash = passwordHasher.Hash("123456Yuiop"),
			RefreshToken = tokensService.GenerateRefreshToken(),
			RefreshTokenExpiryDate = DateTime.Now.AddDays(7),
			CreatedDateUtc = DateTime.UtcNow
		};
		User userSerhii = new User
		{
			Name = "Serhii",
			Email = "serhii.shchoholiev@nure.ua",
			Roles = entitiesRoles,
			PasswordHash = passwordHasher.Hash("123456Yuiop"),
			RefreshToken = tokensService.GenerateRefreshToken(),
			RefreshTokenExpiryDate = DateTime.Now.AddDays(7),
			CreatedDateUtc = DateTime.UtcNow
		};
		User userMykhailo = new User
		{
			Name = "Mykhailo",
			Email = "mykhailo.bilodid@nure.ua",
			Roles = entitiesRoles,
			PasswordHash = passwordHasher.Hash("123456Yuiop"),
			RefreshToken = tokensService.GenerateRefreshToken(),
			RefreshTokenExpiryDate = DateTime.Now.AddDays(7),
			CreatedDateUtc = DateTime.UtcNow
		};
		User userSystem = new User
		{
			Name = "Recipes Manager Team",
			Email = "RecipesManagerTeam@gmail.com",
			Roles = entitiesAdminRoles,
			PasswordHash = passwordHasher.Hash("123456Yuiop"),
			RefreshToken = tokensService.GenerateRefreshToken(),
			RefreshTokenExpiryDate = DateTime.Now.AddDays(7),
			CreatedDateUtc = DateTime.UtcNow
		};
		
		Task.WaitAll(
			usersRepository.AddAsync(userDmytro, CancellationToken.None),
			usersRepository.AddAsync(userVitalii, CancellationToken.None),
			usersRepository.AddAsync(userYelyzaveta, CancellationToken.None),
			usersRepository.AddAsync(userPolina, CancellationToken.None),
			usersRepository.AddAsync(userSerhii, CancellationToken.None),
			usersRepository.AddAsync(userMykhailo, CancellationToken.None),
			usersRepository.AddAsync(userSystem, CancellationToken.None)
		);
		
		GlobalUser.Id = userSystem.Id;
		CategoryCreateDto categoryBreakfast = new CategoryCreateDto { Name = "Breakfast" };
		CategoryCreateDto categoryLunch = new CategoryCreateDto { Name = "Lunch" };
		CategoryCreateDto categoryDinner = new CategoryCreateDto { Name = "Dinner" };
		CategoryCreateDto categoryDessert = new CategoryCreateDto { Name = "Dessert" };
		CategoryCreateDto categorySoup = new CategoryCreateDto { Name = "Soup" };
		CategoryCreateDto categorySalad = new CategoryCreateDto { Name = "Salad" };
		CategoryCreateDto categorySeafood = new CategoryCreateDto { Name = "Seafood" };
		CategoryCreateDto categoryBaking = new CategoryCreateDto { Name = "Baking" };
		CategoryCreateDto categorySauce = new CategoryCreateDto { Name = "Sauce" };
		CategoryCreateDto categoryPizza = new CategoryCreateDto { Name = "Pizza" };
		CategoryCreateDto categoryPasta = new CategoryCreateDto { Name = "Pasta" };
		
		var breakfastTask = categoriesService.AddCategoryAsync(categoryBreakfast, CancellationToken.None);
		var lunchTask = categoriesService.AddCategoryAsync(categoryLunch, CancellationToken.None);
		var dinnerTask = categoriesService.AddCategoryAsync(categoryDinner, CancellationToken.None);
		var dessertTask = categoriesService.AddCategoryAsync(categoryDessert, CancellationToken.None);
		var soupTask = categoriesService.AddCategoryAsync(categorySoup, CancellationToken.None);
		var saladTask = categoriesService.AddCategoryAsync(categorySalad, CancellationToken.None);
		var seafoodTask = categoriesService.AddCategoryAsync(categorySeafood, CancellationToken.None);
		var bakingTask = categoriesService.AddCategoryAsync(categoryBaking, CancellationToken.None);
		var sauceTask = categoriesService.AddCategoryAsync(categorySauce, CancellationToken.None);
		var pizzaTask = categoriesService.AddCategoryAsync(categoryPizza, CancellationToken.None);
		var pastaTask = categoriesService.AddCategoryAsync(categoryPasta, CancellationToken.None);
		
		Task.WaitAll(
			breakfastTask,
			lunchTask,
			dinnerTask,
			dessertTask,
			soupTask,
			saladTask,
			seafoodTask,
			bakingTask,
			sauceTask,
			pizzaTask,
			pastaTask
		);
		
		var breakfastDto = breakfastTask.Result;
		var lunchDto = lunchTask.Result;
		var dinnerDto = dinnerTask.Result;
		var dessertDto = dessertTask.Result;
		var soupDto = soupTask.Result;
		var saladCategoryDto = saladTask.Result;
		var seafoodDto = seafoodTask.Result;
		var bakingDto = bakingTask.Result;
		var sauceDto = sauceTask.Result;
		var pizzaDto = pizzaTask.Result;
		var pastaDto = pastaTask.Result;
		
		RecipeCreateDto sauce1 = new RecipeCreateDto
		{
			Name = "Hogao sauce",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "sauce1.jpg")),
			Text = "Preheat the oven to 220°C/425°F (200°C fan).\n"+
				"Roast tomato 20 min - Toss the tomatoes and garlic with the olive oil in a bowl. Place the tomatoes on the tray, cut side up, then roast for 20 minutes.\n"+
				"Roast garlic 15 min - Add garlic to the tray and roast for another 15 minutes until the edges of the tomato are browned. Remove from oven and cool on tray.\n"+
				"Blitz - Transfer to a container that just fits the head of a stick blender. Add all remaining ingredients except the green onion. Blitz until smooth - about 5 seconds. Add the green onion and blitz until it's finely chopped.\n"+
				"Serve with corn chips, vegetable sticks or bread!",
			IngredientsText = "Tomatoes - 2 medium ones, about 125g/4oz each.\nGarlic cloves - Peel the skin off but keep them whole.\n"+
				"Spices - cumin for flavour and cayenne pepper for a warm buzz.\nSugar - just a tiny amount, to balance the flavour.\n"+
				"Green onion - for a bit of freshness and nice bits of green in the sauce.\n"+
				"Lime juice - for freshness/sour. Substitute with lemon or a vinegar (it's only a small amount so substitutions are flexible).",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Tomatoe", Units = "g", Amount = 250.0 },
				new IngredientDto { Name = "Garlic", Units = "cloves", Amount = 2.0 },
				new IngredientDto { Name = "Extra virgin oil", Units = "tbsp", Amount = 1.0 },
				new IngredientDto { Name = "Ground cumin", Units = "tsp", Amount = 0.125 },
				new IngredientDto { Name = "Ground cayenne", Units = "tsp", Amount = 0.125 },
				new IngredientDto { Name = "Sugar", Units = "tsp", Amount = 0.125 },
				new IngredientDto { Name = "Salt", Units = "tsp", Amount = 0.250 },
				new IngredientDto { Name = "Lime juice", Units = "tsp", Amount = 1.5 },
				new IngredientDto { Name = "Sliced green onion", Units = "tbsp", Amount = 2.0 },
			},
			Categories = new List<CategoryDto> { sauceDto },
			MinutesToCook = 35,
			ServingsCount = 5,
			Calories = 180,
			IsPublic = true
		};
		RecipeCreateDto pasta1 = new RecipeCreateDto
		{
			Name = "Pasta with salmon & peas",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "pasta1.jpg")),
			Text = "Bring a pan of water to the boil and cook the fusilli according to the pack instructions.\n\n"+
				"Meanwhile, heat a knob of butter in a saucepan, then add the shallot and cook for 5 mins or until softened.\n"+
				"Add the peas, salmon, crème fraîche and 50ml water. Crumble in the stock cube.\n"+
				"Cook for 3-4 mins until cooked through, stir in the chives and some black pepper. Then stir through to coat the pasta. Serve in bowls.",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Wholewheat fusilli", Amount = 240.0, Units = "g" },	
				new IngredientDto { Name = "Butter", Amount = 25.0, Units = "g" },	
				new IngredientDto { Name = "Shallot", Amount = 30.0, Units = "g" },	
				new IngredientDto { Name = "Frozen peas", Amount = 140.0, Units = "g" },	
				new IngredientDto { Name = "Skinless salmon fillets", Amount = 200.0, Units = "g" },	
				new IngredientDto { Name = "Low-fat crème fraîche", Amount = 140.0, Units = "g" },	
				new IngredientDto { Name = "Low-salt vegetable stock cube", Amount = 0.5, Units = "cube" },
				new IngredientDto { Name = "Chives", Amount = 15.0, Units = "g" }
			},
			IngredientsText = "240g wholewheat fusilli.\nKnob of butter.\n1 large shallot, finely chopped.\n140g frozen peas.\n2 skinless salmon fillets, cut into chunks.\n"+
				"140g low-fat crème fraîche.\n1 1/2 low-salt vegetable stock cube.\nSmall bunch of chives, snipped.",
			Categories = new List<CategoryDto> { pastaDto,  seafoodDto, dinnerDto },
			Calories = 463,
			ServingsCount = 4,
			MinutesToCook = 15,
			IsPublic = true
		};
		RecipeCreateDto porkRice = new RecipeCreateDto
		{
			Name = "Pork Fried Rice",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "porkRice.jpg")),
			Text = "Melt butter in a large non-stick skillet over medium heat. Add pork, green onion, carrot, and broccoli.\n"+
				"Cook and stir until pork is cooked through, 7 to 10 minutes. Transfer pork mixture to a bowl and return skillet to medium heat.\n\n"+
				"Stir egg into the skillet and scramble until completely set. Add pork mixture back into the skillet;"+
				" stir in rice, peas, soy sauce, garlic powder, and ground ginger. Cook and stir until heated through, 7 to 10 minutes.",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Butter", Amount = 1.0, Units = "tbsp" },	
				new IngredientDto { Name = "Boneless pork loin", Amount = 200.0, Units = "g" },	
				new IngredientDto { Name = "Green onion", Amount = 20.0, Units = "g" },	
				new IngredientDto { Name = "Carrot", Amount = 30.0, Units = "g" },	
				new IngredientDto { Name = "Broccoli", Amount = 30.0, Units = "g" },	
				new IngredientDto { Name = "Egg", Amount = 1.0 },	
				new IngredientDto { Name = "Rice", Amount = 240, Units = "g" },
				new IngredientDto { Name = "Frozen peas", Amount = 0.25, Units = "cup" },
				new IngredientDto { Name = "Soy sauce", Amount = 1.5, Units = "tbsp" },
				new IngredientDto { Name = "Garlic powder", Amount = 0.2, Units = "tsp" },
				new IngredientDto { Name = "Ground ginger", Amount = 0.2, Units = "tsp" },
				
			},
			IngredientsText = "1 tablespoon butter.\n6 ounces boneless pork loin chop, cut into small pieces.\n1 green onion, chopped.\n1/4 cup chopped carrot.\n"+
				"1/4 cup chopped broccoli.\n1 egg, beaten.\n1 cup cold cooked rice.\n1/4 cup frozen peas.\n1 1/2 tablespoons soy sauce.\n1/8 teaspoon garlic powder.\n"+
				"1/8 teaspoon ground ginger.",
			Categories = new List<CategoryDto> { dinnerDto },
			Calories = 996,
			ServingsCount = 2,
			MinutesToCook = 25,
			IsPublic = true
		};
		RecipeCreateDto soup1 = new RecipeCreateDto
		{
			Name = "Pumpkin Soup",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "soup1.jpg")),
			Text = "Cut the pumpkin into 3cm slices. Cut the skin off and scrape seeds out. Cut into 4cm chunks.\n"+
				"Place the pumpkin, onion, garlic, broth and water in a pot - liquid won't quite cover all the pumpkin. "+
				"Bring to a boil, uncovered, then reduce heat and let simmer rapidly until pumpkin is tender (check with butter knife) - about 10 minutes.\n"+
				"Remove from heat and use a stick blender to blend until smooth.\nSeason to taste with salt and pepper, stir through cream (never boil soup after adding cream, cream will split).\n"+
				"Ladle soup into bowls, drizzle over a bit of cream, sprinkle with pepper and parsley if desired. Serve with crusty bread!",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Pumpkin", Amount = 1.2, Units = "kg" },	
				new IngredientDto { Name = "Onion", Amount = 1.0 },	
				new IngredientDto { Name = "Garlic", Amount = 2.0, Units = "cloves" },	
				new IngredientDto { Name = "Vegetable or chicken broth", Amount = 3.0, Units = "cups" },	
				new IngredientDto { Name = "Water", Amount = 1.0, Units = "cup" },	
				new IngredientDto { Name = "Salt", Amount = 0.5, Units = "tbsp" },	
				new IngredientDto { Name = "Paper", Amount = 0.5, Units = "tbsp" }
			},
			IngredientsText = "Pumpkin - Peeled and chopped into large chunks (or purchase it pre-cut).\n"+
				"Onion and garlic - the secret ingredients that adds extra savouriness into the soup flavour!!\n"+
				"Stock/broth and water - for a tastier pumpkin soup, don't skip the broth!\n",
			Categories = new List<CategoryDto> { lunchDto, soupDto },
			Calories = 910,
			ServingsCount = 5,
			MinutesToCook = 15,
			IsPublic = true
		};
		RecipeCreateDto soup2 = new RecipeCreateDto
		{
			Name = "Chinese Noodle Soup",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "soup2.jpg")),
			Text = "Place broth ingredients in a saucepan over high heat. Place lid on, bring to simmer then reduce to medium and simmer for 8 - 10 minutes to allow the flavours to infuse.\n"+
				"Meanwhile, cook noodles according to packet directions.\n"+
				"Cut bok choys in half (for small / medium) or quarter (for large). Wash thoroughly.\n"+
				"Either cook the bok choi in the broth in the soup broth OR noodle cooking water for 1 min (if noodles required boiling).\n"+
				"Pick garlic and ginger out of soup.\n"+
				"Place noodles in bowls. Top with chicken and bok choy. Ladle over soup, garnish with green onions. Great served with chilli paste or fresh chillis.",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Chicken stock", Amount = 3.0, Units = "cups" },	
				new IngredientDto { Name = "Ginger", Amount = 15.0, Units = "g" },	
				new IngredientDto { Name = "Garlic", Amount = 2.0, Units = "cloves" },	
				new IngredientDto { Name = "Soy Sauce", Amount = 1.5, Units = "tbsp" },	
				new IngredientDto { Name = "Sugar", Amount = 2.0, Units = "tsp" },	
				new IngredientDto { Name = "Sesame oil", Amount = 0.5, Units = "tbsp" },	
				new IngredientDto { Name = "Egg noodles", Amount = 180, Units = "g" },
				new IngredientDto { Name = "Bok choy", Amount = 100, Units = "g" },
				new IngredientDto { Name = "Shallot", Amount = 1 }
			},
			IngredientsText = "BROTH:\n3 cups chicken stock/broth, low sodium.\n2 garlic cloves.\n1.5 cm ginger piece.\n1 1/2 tbsp light soy sauce.\n"+
				"2 tsp sugar.\n1 1/2 tbsp chinese cooking wine.\n1/2 tsp sesame oil.\n"+
				"TOPPINGS & NOODLES:\n180g / 6oz fresh egg noodles.\n2 large bok choy or other vegetables of choice.\n1 cup shredded cooked chicken.\n1 scallion / shallot.",
			Categories = new List<CategoryDto> { lunchDto, soupDto },
			Calories = 704,
			ServingsCount = 2,
			MinutesToCook = 15,
			IsPublic = true
		};
		RecipeCreateDto omelet = new RecipeCreateDto
		{
			Name = "Simple Omelette",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "omelet.jpg")),
			Text = "In a small mixing bowl, beat together eggs and a pinch of salt until foamy.\n\n"+
				"Place a small nonstick skillet over medium heat and swirl in 1/2 tbsp butter. Once butter is melted and bubbling, add frothy eggs to the skillet and immediately reduce the heat to low.\n\n"+
				"Use a spatula to pull the cooked eggs into the center, letting the liquid egg fill the space behind it. Continue going around the pan, pulling the eggs towards the center until the eggs are nearly set.\n\n"+
				"Once the omelette is sliding around the pan easily and you can get a spatula underneath, flip the omelette over and turn off the heat.\n\n"+
				"Sprinkle cheese over the egg and add your favorite toppings. Fold the omelette in half and slide it onto your plate then garnish as desired.",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Eggs", Amount = 2.0 },	
				new IngredientDto { Name = "Butter", Amount = 0.5, Units = "tbsp" },	
				new IngredientDto { Name = "Shredded mozzarella", Amount = 0.25, Units = "cup" },	
				new IngredientDto { Name = "Salt", Amount = 0.5, Units = "tbsp" }
			},
			IngredientsText = "Omelette Ingredients:\n\n2 large eggs.\nPinch fine sea salt.\n1/2 tbsp unsalted butter.\n1/4 cup shredded mozzarella, low moisture, part-skim.\n\n"+
				"Optional Fillings:\n\nFresh spinach, coarsely chopped.\nSautéed mushrooms.\nCrispy Air Fryer Bacon, chopped.\nSautéed diced bell pepper and onions.\n"+
				"Ham, diced.\n\nOptional Toppings:\n\nChives or parsley, chopped, to garnish.\nSalt and freshly ground black pepper.\nBaby tomatoes, halved.\n"+
				"Avocado, diced.",
			Categories = new List<CategoryDto> { breakfastDto },
			Calories = 260,
			ServingsCount = 1,
			MinutesToCook = 5,
			IsPublic = true	
		};
		RecipeCreateDto churro = new RecipeCreateDto
		{
			Name = "Baked Churro Bites",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "churro.jpg")),
			Text = "Combine water, butter, brown sugar, and salt in a saucepan over medium heat and bring to a simmer. Add flour all at once and cook, stirring, until a soft, sticky dough ball forms, and a starchy film coats the bottom of the pan, about 5 minutes.\n\n"+
				"Continue cooking for about 1 minute more, scraping the film off the bottom of the pan as you stir. You won't have a clean bottom surface, but some will be reabsorbed by the dough.\n\n"+
				"Transfer to a bowl and let cool for 10 minutes.\n\n"+
				"Add in vanilla extract and eggs, 1 at a time, and mix in using a spatula. The mixture will separate at first, but eventually it will smear together into a sticky dough.\n\n"+
				"Preheat the oven to 425 degrees F (220 degrees C). Line 2 baking sheets with silpat mats or parchment paper.\n\n"+
				"Transfer dough into a pastry bag fitted with a star tip (I used a number 356 tip). Pipe out 3 to 4 inch ropes onto the prepared baking sheets, spaced a few inches apart. "+
				"Use the back of a knife to “cut” through dough at the end of each piping, to get a clean end. Since this makes 28 to 30 bite-sized churros, pipe 14 to 15 onto each baking sheet.\n\n"+
				"Once piped, spray tops of the churros with vegetable oil spray, and then spray water generously all over the pan.\n\n"+
				"Bake in the preheated oven until churros are puffed and the edges are browned, 20 to 25 minutes. Turn off the oven, and open the door for about 10 seconds to vent heat. "+
				"Close the door, leaving it open about 8 to 12 inches, and let churros rest in the oven for 10 minutes.\n\n"+
				"Remove from the oven, and working in batches of 6 at a time, brush them lightly with melted butter, and then toss to coat in a bag of cinnamon sugar.",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Water", Amount = 1.25, Units = "cups"},	
				new IngredientDto { Name = "Butter", Amount = 5, Units = "tbsp" },	
				new IngredientDto { Name = "Brown sugar", Amount = 2, Units = "tbsp" },	
				new IngredientDto { Name = "Salt", Amount = 0.5, Units = "tsp" },
				new IngredientDto { Name = "All-purpose flour", Amount = 1.25, Units = "cups" },
				new IngredientDto { Name = "Vanilla extract", Amount = 1, Units = "tsp" },
				new IngredientDto { Name = "Eggs", Amount = 2 },
				new IngredientDto { Name = "Melted butter", Amount = 2, Units = "tbsp" },
				new IngredientDto { Name = "White sugar", Amount = 0.3, Units = "cup" },
				new IngredientDto { Name = "Cinnamon", Amount = 0.25, Units = "tsp" },
			},
			IngredientsText = "1 1/4 cups water.\n5 tablespoons cold unsalted butter.\n2 packed tablespoons brown sugar.\n1/2 teaspoon salt.\n"+
				"1 1/4 cups all-purpose flour.\n1 teaspoon vanilla extract.\n2 large eggs.\n2 tablespoons melted butter for brushing on before sugaring.\n"+
				"Cooking spray.\n\nCinnamon Sugar:\n\n1/3 cup white sugar.\n1 tablespoon cinnamon.\n1/4 teaspoon salt.",
			Categories = new List<CategoryDto> { dessertDto, bakingDto },	
			Calories = 1800,
			ServingsCount = 30,
			MinutesToCook = 100,
			IsPublic = true
		};
		RecipeCreateDto rolls = new RecipeCreateDto
		{
			Name = "Vietnamese Spring Rolls",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "rolls.jpg")),
			Text = "Fill a large pot with lightly salted water and bring to a rolling boil; stir in vermicelli pasta and return to a boil. Cook pasta uncovered, stirring occasionally, until the pasta is tender yet firm to the bite, 3 to 5 minutes.\n\n"+
				"Fill a large bowl with warm water. Dip one wrapper into the hot water for 1 second to soften. Lay wrapper flat; place 2 shrimp halves in a row across the center, add some vermicelli, lettuce, mint, cilantro, and basil, leaving about 2 "+
				"inches uncovered on each side. Fold uncovered sides inward, then tightly roll the wrapper, beginning at the end with lettuce. Repeat with remaining ingredients.\n\n"+
				"For the sauces: Mix water, lime juice, sugar, fish sauce, garlic, and chili sauce in a small bowl until well combined. Mix hoisin sauce and peanuts in a separate small bowl.\n\n"+
				"Serve rolled spring rolls with fish sauce and hoisin sauce mixtures.",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Rrice vermicelli", Amount = 2, Units = "oz" },	
				new IngredientDto { Name = "Rice wrappers", Amount = 8 },	
				new IngredientDto { Name = "Large cooked shrimp", Amount = 8 },	
				new IngredientDto { Name = "Lettuce", Amount = 2, Units = "leaves" },
				new IngredientDto { Name = "Mint leaves", Amount = 3, Units = "tbsp" },
				new IngredientDto { Name = "Cilantro", Amount = 3, Units = "tbsp" },
				new IngredientDto { Name = "Thai basil", Amount = 1.3, Units = "tbsp" },
				new IngredientDto { Name = "Water", Amount = 0.25, Units = "cup" },
				new IngredientDto { Name = "Lime juice", Amount = 2, Units = "tbsp" },
				new IngredientDto { Name = "White sugar", Amount = 2, Units = "tbsp" },
				new IngredientDto { Name = "Fish sauce", Amount = 4, Units = "tsp" },
				new IngredientDto { Name = "Garlic", Amount = 1, Units = "clove" },
				new IngredientDto { Name = "Garlic chili sauce", Amount = 0.5, Units = "tsp" },
				new IngredientDto { Name = "Hoisin sauce", Amount = 3, Units = "tbsp" },
				new IngredientDto { Name = "Chopped peanuts", Amount = 1, Units = "tsp" },
			},
			IngredientsText = "2 ounces rice vermicelli.\n8 rice wrappers (8.5 inch diameter).\n8 large cooked shrimp - peeled, deveined and cut in half.\n"+
				"2 leaves lettuce, chopped.\n3 tablespoons chopped fresh mint leaves.\n3 tablespoons chopped fresh cilantro.\n1 1/3 tablespoons chopped fresh Thai basil.\n\n"+
				"Sauces:\n\n1/4 cup water.\n2 tablespoons fresh lime juice.\n2 tablespoons white sugar.\n4 teaspoons fish sauce.\n1 clove garlic, minced.\n"+
				"1/2 teaspoon garlic chili sauce.\n3 tablespoons hoisin sauce.\n1 teaspoon finely chopped peanuts.",
			Categories = new List<CategoryDto> { seafoodDto, lunchDto },
			Calories = 656,
			ServingsCount = 8,
			MinutesToCook = 50,
			IsPublic = true		
		};
		RecipeCreateDto bakedChicken = new RecipeCreateDto
		{
			Name = "Baked Chicken Breasts",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "bakedChicken.jpg")),
			Text = "Gather all ingredients and preheat the oven to 400 degrees F (200 degrees C).\n\n"+
				"Rub chicken breasts with olive oil and sprinkle both sides with salt and Creole seasoning. Place chicken in a broiler pan.\n\n"+
				"Bake in the preheated oven for 10 minutes. Flip chicken and cook until no longer pink in the center and the juices run clear, about 15 minutes more. An instant-read thermometer inserted into the center should read at least 165 degrees F (74 degrees C).\n\n"+
				"Remove chicken to a plate.\n\nPour chicken broth into the pan and scrape any browned bits off the bottom with a flat-edged wooden spatula. Add more broth if needed to dislodge the browned bits, but not too much or it will be watery.\n\n"+
				"To serve, drizzle the pan sauce over the chicken.",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Chicken breast halves", Amount = 5, Units = "oz" },	
				new IngredientDto { Name = "Olive oil", Amount = 2, Units = "tbsp" },	
				new IngredientDto { Name = "Coarse sea salt", Amount = 0.5, Units = "tsp" },	
				new IngredientDto { Name = "Creole seasoning", Amount = 0.1, Units = "tsp" },
				new IngredientDto { Name = "Chicken broth", Amount = 1, Units = "tbsp" }
			},
			IngredientsText = "4 (5 ounce) skinless, boneless chicken breast halves.\n2 tablespoons olive oil.\n1/2 teaspoon coarse sea salt, or to taste.\n"+
				"1 pinch Creole seasoning, or more to taste.\n1 tablespoon chicken broth, or more to taste.",
			Categories = new List<CategoryDto> { dinnerDto },
			Calories = 764,
			ServingsCount = 4,
			MinutesToCook = 30,
			IsPublic = true
		};
		RecipeCreateDto pizza1 = new RecipeCreateDto
		{
			Name = "Pizza Margherita",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "pizza1.jpg")),
			Text = "Make the base: Put the flour into a large bowl, then stir in the yeast and salt. Make a well, pour in 200ml warm water and the olive oil and bring together with a wooden spoon until you have a soft, fairly wet dough. "+
				"Turn onto a lightly floured surface and knead for 5 mins until smooth. Cover with a tea towel and set aside. You can leave the dough to rise if you like, but it's not essential for a thin crust.\n\n"+
				"Make the sauce: Mix the passata, basil and crushed garlic together, then season to taste. Leave to stand at room temperature while you get on with shaping the base.\n\n"+
				"Roll out the dough: if you've let the dough rise, give it a quick knead, then split into two balls. On a floured surface, roll out the dough into large rounds, about 25cm across, using a rolling pin. The dough needs to be very thin as it will rise in the oven. Lift the rounds onto two floured baking sheets.\n\n"+
				"Top and bake: heat the oven to 240C/220C fan/gas 8. Put another baking sheet or an upturned baking tray in the oven on the top shelf. Smooth sauce over bases with the back of a spoon. "+
				"Scatter with cheese and tomatoes, drizzle with olive oil and season. Put one pizza, still on its baking sheet, on top of the preheated sheet or tray. Bake for 8-10 mins until crisp. Serve with a little more olive oil, and basil leaves if using. Repeat step for remaining pizza.",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Strong bread flour", Amount = 300, Units = "g" },	
				new IngredientDto { Name = "Instant yeast", Amount = 1, Units = "tsp" },	
				new IngredientDto { Name = "Salt", Amount = 1, Units = "tsp" },	
				new IngredientDto { Name = "Olive oil", Amount = 1, Units = "tbsp" },
				new IngredientDto { Name = "Passata", Amount = 100, Units = "ml" },
				new IngredientDto { Name = "Fresh basil", Amount = 1, Units = "tsp" },
				new IngredientDto { Name = "Garlic", Amount = 1, Units = "clove" },
				new IngredientDto { Name = "Mozzarella", Amount = 125, Units = "g" },
				new IngredientDto { Name = "Cherry tomatoes", Amount = 40, Units = "g" },
			},
			IngredientsText = "For the base:\n\n300g strong bread flour.\n1 tsp instant yeast (from a sachet or a tub).\n1 tsp salt.\n1 tbsp olive oil, plus extra for drizzling.\n\n"+
				"For the tomato sauce:\n\n100ml passata.\nHandful fresh basil or 1 tsp dried.\n1 garlic clove, crushed.\n\nFor the topping:\n\n"+
				"125g ball mozzarella, sliced.\nHandful grated or shaved parmesan (or vegetarian alternative).\nHandful of cherry tomatoes, halved.\n\n"+
				"To finish:\n\nHandful of basil leaves (optional).",
			Categories = new List<CategoryDto> { pizzaDto, dinnerDto },
			Calories = 1567,
			ServingsCount = 4,
			MinutesToCook = 30,
			IsPublic = true	
		};
		RecipeCreateDto ciabatta= new RecipeCreateDto
		{
			Name = "Garlic & basil ciabatta",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "ciabatta.jpg")),
			Text = "Heat the grill to high. Beat together the mayonnaise, butter and garlic cloves until smooth. Chop basil, stir through and season with salt and pepper. Put the halves of the ciabatta, sliced lengthways, on a baking tray and spread with the butter. Sprinkle with grated Parmesan, then grill for 2-3 mins.",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Mayonnaise", Amount = 1, Units = "tbsp" },	
				new IngredientDto { Name = "Butter", Amount = 2, Units = "tbsp" },	
				new IngredientDto { Name = "Basil", Amount = 1, Units = "bunch" },	
				new IngredientDto { Name = "Ciabatta", Amount = 1 },
				new IngredientDto { Name = "Grated parmesan", Amount = 2, Units = "tbsp" },
				new IngredientDto { Name = "Garlic", Amount = 2, Units = "cloves" }
			},
			IngredientsText = "1 tbsp mayonnaise.\n2 tbsp butter, softened.\n1 bunch of basil.\n1 small ciabatta.\n2 tbsp grated parmesan (or vegetarian alternative).\n2 garlic cloves ,crushed.",
			Categories = new List<CategoryDto> { breakfastDto }, 
			Calories = 324,
			ServingsCount = 4,
			MinutesToCook = 5,
			IsPublic = true
		};
		RecipeCreateDto pasta2 = new RecipeCreateDto
		{
			Name = "Pea & ham pasta",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "pasta2.jpg")),
			Text = "Bring a large pan of salted water to the boil, then cook the pasta following pack instructions, about 9 mins. Around 3 mins before the pasta is cooked, add the peas.\n\n"+
				"Meanwhile, heat the olive oil in a frying pan over a medium heat and fry the onion for 5 mins until soft but not golden. Add the ham, double cream, lemon juice and parmesan, then season and mix well. Remove from the heat.\n\n"+
				"Drain the pasta and peas (keeping a mugful of the cooking water) and return them to the pan. Tip in the creamy ham mixture and stir everything together to combine. If the mixture seems a little dry, add some of the reserved cooking water to loosen it a little. Serve with extra parmesan on top, if you like.",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Conchiglione pasta", Amount = 200, Units = "g" },	
				new IngredientDto { Name = "Podded peas", Amount = 160, Units = "g" },	
				new IngredientDto { Name = "Olive oil", Amount = 1, Units = "tbsp" },	
				new IngredientDto { Name = "Red onion", Amount = 1 },
				new IngredientDto { Name = "Cooked ham", Amount = 100, Units = "g" },
				new IngredientDto { Name = "Double cream", Amount = 150, Units = "ml" },
				new IngredientDto { Name = "Lemon juice", Amount = 12, Units = "ml" },
				new IngredientDto { Name = "Parmesan", Amount = 40, Units = "g" }
			},	
			IngredientsText = "200g conchiglione pasta.\n160g podded peas.\n1 tbsp olive oil.\n1 red onion, finely chopped.\n100g cooked ham.\n"+
				"150ml double cream.\n1/2 lemon, juiced.\n40g parmesan, grated, plus extra to serve.",
			Categories = new List<CategoryDto> { pastaDto, dinnerDto }, 
			Calories = 1038,
			ServingsCount = 2,
			MinutesToCook = 10,
			IsPublic = true
		};
		RecipeCreateDto salad = new RecipeCreateDto
		{
			Name = "Greek salad",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "salad1.jpg")),
			Text = "Place 4 large vine tomatoes, cut into wedges, 1 peeled, deseeded and chopped cucumber, 1/2 a thinly sliced red onion, "+
				"16 Kalamata olives, 1 tsp dried oregano, 85g feta cheese chunks and 4 tbsp Greek extra virgin olive oil in a large bowl.\n"+
				"Lightly season, then serve with crusty bread to mop up all of the juices.",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Vine tomatoes", Amount = 4 },	
				new IngredientDto { Name = "Cucumber", Amount = 1 },	
				new IngredientDto { Name = "Red onion ", Amount = 0.5 },	
				new IngredientDto { Name = "Kalamata olives", Amount = 16 },
				new IngredientDto { Name = "Dried oregano", Amount = 1, Units = "tsp" },
				new IngredientDto { Name = "Feta cheese,", Amount = 85, Units = "g" },
				new IngredientDto { Name = "Olive oil", Amount = 4, Units = "tbsp" }
			},		
			IngredientsText = "4 large vine tomatoes, cut into irregular wedges.\n1 cucumber, peeled, deseeded, then roughly chopped.\n1/2 a red onion thinly sliced.\n"+
				"16 Kalamata olives.\n1 tsp dried oregano.\n85g feta cheese, cut into chunks (barrel matured feta is the best).\n4 tbsp Greek extra virgin olive oil.",
			Categories = new List<CategoryDto> { saladCategoryDto, lunchDto }, 	
			Calories = 270,
			ServingsCount = 2,
			MinutesToCook = 15,
			IsPublic = true
		};
		RecipeCreateDto muffin = new RecipeCreateDto
		{
			Name = "Simple muffins",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "muffin.jpg")),
			Text = "Heat oven to 200C/180C fan/gas 6. Line 2 muffin trays with paper muffin cases. In a large bowl beat 2 medium eggs lightly with a handheld electric mixer for 1 min.\n\n"+
				"Add 125ml vegetable oil and 250ml semi-skimmed milk and beat until just combined then add 250g golden caster sugar and whisk until you have a smooth batter.\n\n"+
				"Sift in 400g self-raising flour and 1 tsp salt (or 400g plain flour and 3 tsp baking powder if using) then mix until just smooth. Be careful not to over-mix the batter as this will make the muffins tough.\n\n"+
				"Stir in 100g chocolate chips or dried fruit if using.\n\n"+
				"Fill muffin cases two-thirds full and bake for 20-25 mins, until risen, firm to the touch and a skewer inserted in the middle comes out clean. If the trays will not fit on 1 shelf, swap the shelves around after 15 mins of cooking.\n\n"+
				"Leave the muffins in the tin to cool for a few mins and transfer to a wire rack to cool completely.",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Eggs", Amount = 2 },	
				new IngredientDto { Name = "Vegetable oil", Amount = 125, Units = "ml" },	
				new IngredientDto { Name = "Semi-skimmed milk", Amount = 250, Units = "ml" },	
				new IngredientDto { Name = "Golden caster sugar", Amount = 250, Units = "g" },
				new IngredientDto { Name = "Self-raising flour", Amount = 400, Units = "g" },
				new IngredientDto { Name = "Salt", Amount = 1, Units = "tsp" },
				new IngredientDto { Name = "Chocolate chips", Amount = 100, Units = "g" }
			},	
			IngredientsText = "2 medium eggs.\n125ml vegetable oil.\n250ml semi-skimmed milk.\n250g golden caster sugar.\n400g self-raising flour (or same quantity plain flour and 3 tsp baking powder).\n"+
				"1 tsp salt.\n100g chocolate chips or dried fruit such as sultanas or dried cherries (optional).",
			Categories = new List<CategoryDto> { dessertDto, bakingDto },
			Calories = 4100,
			ServingsCount = 20,
			MinutesToCook = 35,
			IsPublic = true
		};
		RecipeCreateDto tacos = new RecipeCreateDto
		{
			Name = "Fish tacos",
			Thumbnail = ConvertJpegToFormFile(Path.Combine(thumbnailsPath, "tacos.jpg")),
			Text = "Combine the cumin, coriander, paprika and a generous pinch of salt in a large bowl, add the juice from 1 lime and mix well. Toss the fish fillets in the spiced lime paste and set aside while you prepare the salad.\n\n"+
				"Finely slice the cabbage - you can do this by hand or in a food processor if you want it really fine - squeeze over the juice from half a lime and season with a little salt, scrunch the salt and lime into the cabbage and set aside. "+
				"Chop the tomatoes into small pieces. Stone the avocado, scoop out the soft inside and slice, or if it's a very ripe avocado you may want to mash it in a bowl with a little lime and salt. Keep all the salad ingredients separate on a board.\n\n"+
				"Heat the grill to high. Line a baking tray with foil and brush with a little oil, place the fish fillets on the tray, pour over any paste from the bowl and brush with a little more oil. "+
				"Cook the fish close to the grill for 8-10 mins until the fish is cooked and starting to scorch in places, to check its cooked, gently push one of the fillets, it should easily flake when cooked.\n\n"+
				"If you have a gas hob, warm the tortillas directly over the flames with a pair of tongs for a charred finish. Alternatively, wrap in foil and warm in the oven on the shelf beneath the fish.\n\n"+
				"To serve, spread a little soured cream over each warm tortilla, top with a handful of cabbage, some tomatoes and a few slices of avocado. Flake the fish and add a few big chunks to each tortilla then top with coriander, chilli and chilli sauce.",
			Ingredients = new List<IngredientDto>
			{
				new IngredientDto { Name = "Ground cumin", Amount = 1, Units = "tsp" },	
				new IngredientDto { Name = "Ground coriander", Amount = 1, Units = "tsp" },	
				new IngredientDto { Name = "Smoked paprika", Amount = 2, Units = "tsp" },	
				new IngredientDto { Name = "Limes", Amount = 2 },	
				new IngredientDto { Name = "White fish fillets", Amount = 500, Units = "g" },
				new IngredientDto { Name = "Red cabbage", Amount = 0.25 },
				new IngredientDto { Name = "Tomatoes", Amount = 2 },
				new IngredientDto { Name = "Avocados", Amount = 2 },
				new IngredientDto { Name = "Vegetable oil", Amount = 2, Units = "tbsp" },
				new IngredientDto { Name = "Tortilla wraps", Amount = 8 },
				new IngredientDto { Name = "Green chilli", Amount = 1 },
				new IngredientDto { Name = "Soured cream", Amount = 100, Units = "g" },
				new IngredientDto { Name = "Chilli sauce", Amount = 100, Units = "g" }
			},
			IngredientsText = "1 tsp ground cumin.\n1 tsp ground coriander.\n2 tsp smoked paprika.\n2 limes.\n"+
				"500g white fish fillets, such as cod, haddock, pollack or tilapia, skin and bones removed.\n1/4 red cabbage.\n"+
				"2 large tomatoes.\n2 large avocados.\n2 tbsp vegetable oil.\n8 small corn or wheat tortilla wraps.\n"+
				"Small bunch coriander, chopped.\n1 green chilli, finely sliced, optional.\n100g soured cream.\nChilli sauce, to serve.",
			Categories = new List<CategoryDto> { lunchDto },
			Calories = 2660,
			ServingsCount = 4,
			MinutesToCook = 20,
			IsPublic = true	
		};
		
		GlobalUser.Id = userDmytro.Id;
		var sauce1Dto = recipesService.AddRecipeAsync(sauce1, CancellationToken.None).Result;
		var pasta1Dto = recipesService.AddRecipeAsync(pasta1, CancellationToken.None).Result;
		GlobalUser.Id = userMykhailo.Id;
		var porkRiceDto = recipesService.AddRecipeAsync(porkRice, CancellationToken.None).Result;
		var soup1Dto = recipesService.AddRecipeAsync(soup1, CancellationToken.None).Result;
		GlobalUser.Id = userPolina.Id;
		var soup2Dto = recipesService.AddRecipeAsync(soup2, CancellationToken.None).Result;
		var omeletDto = recipesService.AddRecipeAsync(omelet, CancellationToken.None).Result;
		GlobalUser.Id = userSerhii.Id;
		var churroDto = recipesService.AddRecipeAsync(churro, CancellationToken.None).Result;
		var rollsDto = recipesService.AddRecipeAsync(rolls, CancellationToken.None).Result;
		var bakedChickenDto = recipesService.AddRecipeAsync(bakedChicken, CancellationToken.None).Result;
		GlobalUser.Id = userYelyzaveta.Id;
		var pizza1Dto = recipesService.AddRecipeAsync(pizza1, CancellationToken.None).Result;
		var ciabattaDto = recipesService.AddRecipeAsync(ciabatta, CancellationToken.None).Result;
		var pasta2Dto = recipesService.AddRecipeAsync(pasta2, CancellationToken.None).Result;
		GlobalUser.Id = userVitalii.Id;
		var saladDto = recipesService.AddRecipeAsync(salad, CancellationToken.None).Result;
		var muffinDto = recipesService.AddRecipeAsync(muffin, CancellationToken.None).Result;
		var tacosDto = recipesService.AddRecipeAsync(tacos, CancellationToken.None).Result;
		
		GlobalUser.Id = userSystem.Id;
		SavedRecipeCreateDto savedRecipe1 = new SavedRecipeCreateDto { RecipeId = pasta1Dto.Id };
		SavedRecipeCreateDto savedRecipe2 = new SavedRecipeCreateDto { RecipeId = pizzaDto.Id };
		SavedRecipeCreateDto savedRecipe3 = new SavedRecipeCreateDto { RecipeId = soup1Dto.Id };
		SavedRecipeCreateDto savedRecipe4 = new SavedRecipeCreateDto { RecipeId = tacosDto.Id };
		SavedRecipeCreateDto savedRecipe5 = new SavedRecipeCreateDto { RecipeId = muffinDto.Id };
		Task.WaitAll(
			savedRecipesService.AddSavedRecipeAsync(savedRecipe1, CancellationToken.None),
			savedRecipesService.AddSavedRecipeAsync(savedRecipe2, CancellationToken.None),
			savedRecipesService.AddSavedRecipeAsync(savedRecipe3, CancellationToken.None),
			savedRecipesService.AddSavedRecipeAsync(savedRecipe4, CancellationToken.None),
			savedRecipesService.AddSavedRecipeAsync(savedRecipe5, CancellationToken.None)
		);
		
		SubscriptionCreateDto subscription1 = new SubscriptionCreateDto { AuthorId = userVitalii.Id.ToString() };
		SubscriptionCreateDto subscription2 = new SubscriptionCreateDto { AuthorId = userYelyzaveta.Id.ToString() };
		Task.WaitAll(
			subscriptionService.AddSubscriptionAsync(subscription1, CancellationToken.None),
			subscriptionService.AddSubscriptionAsync(subscription2, CancellationToken.None)
		);
		
		MenuCreateDto menu = new MenuCreateDto
		{
			Name = "International Delights",
			RecipesIds = new List<string> { porkRiceDto.Id, rollsDto.Id, tacosDto.Id, churroDto.Id },
			Notes = "Experience a world of culinary wonders with our International Delights menu, where flavors from around the globe converge to create an unforgettable dining experience. "+
				"Indulge in a tantalizing assortment of dishes that will transport your taste buds on an international journey, satisfying your cravings for diverse and delicious cuisine. "+
				"From the aromatic spices of Asia to the vibrant street food of Mexico. Each dish is thoughtfully crafted to bring you an authentic taste experience. "+
				"Embark on a culinary journey like no other and let our International Delights menu take you on a tantalizing adventure around the globe",
			ForDateUtc = DateTime.UtcNow
		};
		menusService.AddMenuAsync(menu, CancellationToken.None).Wait();
		
		ShoppingListCreateDto shoppingList = new ShoppingListCreateDto
		{
			Name = "International Delights",
			RecipesIds = new List<string> { porkRiceDto.Id, rollsDto.Id, tacosDto.Id, churroDto.Id },
		};
		shoppingListsService.AddShoppingListAsync(shoppingList, CancellationToken.None).Wait();
		
		Thread.Sleep(4444);
		
		if (serviceProvider is IDisposable disposable)
		{
			disposable.Dispose();
		}
	}
	
	private static IFormFile ConvertJpegToFormFile(string path)
	{
		byte[] bytes = File.ReadAllBytes(path);
		string name = Path.GetFileName(path);
		string type = "image/jpeg";
		
		return new FormFile(new MemoryStream(bytes), 0, bytes.Length, name, name)
		{
			Headers = new HeaderDictionary(),
			ContentType = type
		};
	}
}