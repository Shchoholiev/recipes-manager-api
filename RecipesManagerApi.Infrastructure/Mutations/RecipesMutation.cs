using RecipesManagerApi.Application.IServices;
using HotChocolate.Authorization;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class RecipesMutation
{
    [Authorize]
    public Task<OperationDetails> DeleteRecipeAsync(string id, CancellationToken cancellationToken,
        [Service] IRecipesService service)
        => service.DeleteAsync(id, cancellationToken);
}

