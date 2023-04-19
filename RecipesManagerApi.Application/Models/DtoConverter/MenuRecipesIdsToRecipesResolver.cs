using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.Models.DtoConverter;

public class MenuRecipesIdsToRecipesResolver : IValueResolver<Menu, MenuDto, List<RecipeDto>>
{
	private readonly IRecipesRepository _recipeRepository;
	private readonly IMapper _mapper;
	
	public MenuRecipesIdsToRecipesResolver(IRecipesRepository recipesRepository, IMapper mapper)
	{
		_recipeRepository = recipesRepository;
		_mapper = mapper;
	}

    public List<RecipeDto> Resolve(Menu source, MenuDto destination, List<RecipeDto> destMember, ResolutionContext context)
    {
        if(source.RecipesIds == null)
		{
			return new List<RecipeDto>();
		}
		
		var recipes = new List<Recipe>();
		foreach(var recipeId in source.RecipesIds)
		{
			recipes.Add(_recipeRepository.GetRecipeAsync(recipeId, CancellationToken.None).Result);
		}
		
		return this._mapper.Map<List<RecipeDto>>(recipes);
    }
}