using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Enums;

namespace RecipesManagerApi.Application.IServices;

public interface IRecipesService
{
    Task AddRecipeAsync(RecipeCreateDto dto, CancellationToken cancellationToken);

    Task UpdateRecipeAsync(RecipeDto dto, CancellationToken cancellationToken);

    Task DeleteRecipeAsync(RecipeDto dto, CancellationToken cancellationToken);

    Task<RecipeDto> GetRecipeAsync(string id, CancellationToken cancellationToken);

    Task<PagedList<RecipeDto>> GetRecipesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<PagedList<RecipeDto>> GetSearchPageAsync(int pageNumber, int pageSize, string searchString, string? authorId,
        List<string>? categoriesIds, RecipesSearchTypes recipeSearchType, CancellationToken cancellationToken);
}
