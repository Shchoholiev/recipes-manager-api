using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Paging;
using HotChocolate.Authorization;
using RecipesManagerApi.Application.Models.Dtos;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class SavedRecipesQuery
{
    [Authorize]
    public Task<PagedList<SavedRecipeDto>> GetSavedRecipesAsync(int pageNumber, int pageSize, string userId, CancellationToken cancellationToken,
        [Service] ISavedRecipesService service)
        => service.GetSavedRecipesPageAsync(pageNumber, pageSize, userId, cancellationToken);

    [Authorize]
    public Task<SavedRecipeDto> GetSavedRecipeAsync(string id, CancellationToken cancellationToken,
        [Service] ISavedRecipesService service)
        => service.GetSavedRecipeAsync(id, cancellationToken);
}

