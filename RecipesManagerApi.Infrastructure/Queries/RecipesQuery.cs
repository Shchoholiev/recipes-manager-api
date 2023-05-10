using MongoDB.Bson;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Enums;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class RecipesQuery
{
    public Task<RecipeDto> GetRecipeAsync(string id, CancellationToken cancellationToken,
        [Service] IRecipesService service)
        => service.GetRecipeAsync(id ,cancellationToken);

    public Task<PagedList<RecipeDto>> GetRecipesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
        [Service] IRecipesService service)
        => service.GetRecipesPageAsync(pageNumber, pageSize, cancellationToken);

    public Task<PagedList<RecipeDto>> GetRecipeSearchResultAsync(int pageNumber, int pageSize, string searchString, string? authorId,
     List<string>? categoriesIds, RecipesSearchTypes? recipeSearchType, CancellationToken cancellationToken,
    [Service] IRecipesService service)
        => service.GetSearchPageAsync(pageNumber, pageSize, searchString, authorId, categoriesIds, recipeSearchType, cancellationToken);
}

