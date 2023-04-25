using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class SavedRecipesQuery
{
    public Task<PagedList<SavedRecipeDto>> GetSavedRecipesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
        [Service] ISavedRecipesService service)
        => service.GetSavedRecipesPageAsync(pageNumber, pageSize, cancellationToken);

    public Task<SavedRecipeDto> GetSavedRecipeAsync(string id, CancellationToken cancellationToken,
        [Service] ISavedRecipesService service)
        => service.GetSavedRecipeAsync(id, cancellationToken);
}

