using RecipesManagerApi.Application.IServices;
using HotChocolate.Authorization;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class RecipesMutation
{
    [Authorize]
    public Task DeleteRecipeAsync(string id, CancellationToken cancellationToken,
        [Service] IRecipesService service)
        => service.DeleteAsync(id, cancellationToken);
}

