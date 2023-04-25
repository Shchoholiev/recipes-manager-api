using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class RecipesMutation
{
    public Task DeleteRecipeAsync(RecipeDto recipe, CancellationToken cancellationToken,
         [Service] IRecipesService service)
         => service.DeleteRecipeAsync(recipe, cancellationToken);
}

