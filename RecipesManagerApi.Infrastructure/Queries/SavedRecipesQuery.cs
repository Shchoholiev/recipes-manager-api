using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;
using HotChocolate.Authorization;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class SavedRecipesQuery
{
    [Authorize]
    public Task<PagedList<SavedRecipeDto>> GetSavedRecipesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
        [Service] ISavedRecipesService service)
        => service.GetSavedRecipesPageAsync(pageNumber, pageSize, cancellationToken);

    [Authorize]
    public Task<SavedRecipeDto> GetSavedRecipeAsync(string id, CancellationToken cancellationToken,
        [Service] ISavedRecipesService service)
        => service.GetSavedRecipeAsync(id, cancellationToken);
}

