using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;

public interface ISharedRecipesRepository : IBaseRepository<SharedRecipe>
{
	Task<SharedRecipe> UpdateSharedRecipeAsync(ObjectId id, SharedRecipe recipe, CancellationToken cancellationToken);
	
	Task<SharedRecipe> UpdateSharedRecipeVisitsAsync(ObjectId id, SharedRecipe recipe, CancellationToken cancellationToken);

	Task<SharedRecipe> GetSharedRecipeAsync(ObjectId id, CancellationToken cancellationToken);
}
