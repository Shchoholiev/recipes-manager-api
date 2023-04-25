using MongoDB.Bson;
using System.Linq.Expressions;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;

public interface ISavedRecipesRepository : IBaseRepository<SavedRecipe>
{
    Task<SavedRecipe> GetSavedRecipeAsync(ObjectId id, CancellationToken cancellationToken);

    Task UpdateSavedRecipeAsync(SavedRecipe recipe, CancellationToken cancellationToken);

    Task<int> GetTotalCountAsync(Expression<Func<SavedRecipe, bool>> predicate);
}

