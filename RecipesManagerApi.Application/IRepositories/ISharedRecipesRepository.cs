using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;

public interface ISharedRecipesRepository : IBaseRepository<SharedRecipe>
{
    Task UpdateSharedRecipeAsync(SharedRecipe recipe, CancellationToken cancellationToken);

    Task<SharedRecipe> GetSharedRecipeAsync(ObjectId id, CancellationToken cancellationToken);
}
