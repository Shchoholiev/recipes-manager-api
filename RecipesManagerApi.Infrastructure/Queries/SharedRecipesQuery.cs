using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class SharedRecipesQuery
{
    public Task<SharedRecipeDto> AccessSharedRecipeAsync(string id, CancellationToken cancellationToken,
    [Service] ISharedRecipesService recipesService)
    => recipesService.AccessSharedRecipeAsync(id, cancellationToken);

    public Task<SharedRecipeDto> GetSharedRecipeAsync(string id, CancellationToken cancellationToken,
    [Service] ISharedRecipesService recipesService)
    => recipesService.GetSharedRecipeAsync(id, cancellationToken);
}
