using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Enums;

namespace RecipesManagerApi.Application.IServices;

public interface IRecipesService
{
    Task AddRecipeAsync(RecipeCreateDto dto, CancellationToken cancellationToken);

    Task UpdateRecipeAsync(RecipeDto dto, CancellationToken cancellationToken);

    Task<RecipeDto> GetRecipeAsync(ObjectId id, CancellationToken cancellationToken);    

    Task<PagedList<RecipeDto>> GetSearchPageAsync(int pageNumber, int pageSize, string searchString, RecipesSearchTypes? recipeSearchType, ObjectId userId, CancellationToken cancellationToken;
}
