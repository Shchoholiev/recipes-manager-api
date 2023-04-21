using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class SharedRecipesMutation
{
    public Task<SharedRecipeDto> AddSharedRecipeAsync(SharedRecipeCreateDto dto, CancellationToken cancellationToken,
    [Service] ISharedRecipesService recipesService)
    => recipesService.AddSharedRecipeAsync(dto, cancellationToken);

    public Task<OperationDetails> UpdateSharedRecipeAsync(SharedRecipeDto dto, CancellationToken cancellationToken,
    [Service] ISharedRecipesService recipesService)
    => recipesService.UpdateSharedRecipeAsync(dto, cancellationToken);
}
