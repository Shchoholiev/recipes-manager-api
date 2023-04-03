using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Enums;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IServices;

public interface IRecipesService
{
    Task AddRecipeAsync(RecipeDto dto, CancellationToken cancellationToken);

    Task UpdateRecipeAsync(RecipeDto dto, CancellationToken cancellationToken);

    Task<RecipeDto> GetRecipeAsync(ObjectId id, CancellationToken cancellationToken);    

    Task<PagedList<RecipeDto>> GetSearchPageAsync(int pageNumber, int pageSize, RecipesSearchTypes recipeSearchType, ObjectId userId, CancellationToken cancellationToken);

    //Task<PagedList<RecipeDto>> search (1.recipe type, 2.text(name, ingredients, category(if null))optional private,
    // 3.categories optional)
}
