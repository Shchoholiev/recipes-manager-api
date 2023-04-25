using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class SavedRecipesMutation
{
    public Task<SavedRecipeDto> AddSavedRecipeAsync(SavedRecipeCreateDto dto, CancellationToken cancellationToken,
     [Service] ISavedRecipesService recipesService)
     => recipesService.AddSavedRecipeAsync(dto, cancellationToken);

    public Task<OperationDetails> DeleteSavedRecipeAsync(SavedRecipeDto dto, CancellationToken cancellationToken,
    [Service] ISavedRecipesService recipesService)
    => recipesService.DeleteSavedRecipeAsync(dto, cancellationToken);
}

