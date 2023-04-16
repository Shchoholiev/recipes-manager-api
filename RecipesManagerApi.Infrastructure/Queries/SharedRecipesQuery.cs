using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class SharedRecipesQuery
{
    public Task<SharedRecipeDto> AccessSharedRecipeAsync(SharedRecipeDto dto, CancellationToken cancellationToken,
    [Service] ISharedRecipesService recipesService)
    => recipesService.AccessSharedRecipeAsync(dto, cancellationToken);

    public Task<SharedRecipeDto> GetSharedRecipeAsync(string id, CancellationToken cancellationToken,
    [Service] ISharedRecipesService recipesService)
    => recipesService.GetSharedRecipeAsync(id, cancellationToken);
}
