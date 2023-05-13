using HotChocolate.Authorization;
using MongoDB.Bson;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Enums;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class RecipesQuery
{
    [Authorize]
    public Task<RecipeDto> GetRecipeAsync(string id, CancellationToken cancellationToken,
        [Service] IRecipesService service)
        => service.GetRecipeAsync(id ,cancellationToken);

    [Authorize]
    public Task<PagedList<RecipeDto>> GetRecipesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
        [Service] IRecipesService service)
        => service.GetRecipesPageAsync(pageNumber, pageSize, cancellationToken);

    [Authorize]
    public Task<PagedList<RecipeDto>> SearchRecipesAsync([Service] IRecipesService service,
        RecipesSearchTypes recipeSearchType, List<string>? categoriesIds, CancellationToken cancellationToken,
        int pageNumber = 1, int pageSize = 10, string searchString = "", string authorId = "")
        => service.GetSearchPageAsync(pageNumber, pageSize, searchString, authorId, categoriesIds, recipeSearchType, cancellationToken);
}

