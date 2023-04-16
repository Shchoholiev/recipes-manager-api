using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class SharedRecipesMutation
{
    public Task AddSharedRecipeAsync(SharedRecipeDto dto, CancellationToken cancellationToken,
    [Service] ISharedRecipesService recipesService)
    => recipesService.AddSharedRecipeAsync(dto, cancellationToken);

    public Task UpdateSharedRecipeAsync(SharedRecipeDto dto, CancellationToken cancellationToken,
    [Service] ISharedRecipesService recipesService)
    => recipesService.UpdateSharedRecipeAsync(dto, cancellationToken);
}
