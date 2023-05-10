using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IRepositories;

public interface IRecipesRepository : IBaseRepository<Recipe>
{
    Task<Recipe> GetRecipeAsync(ObjectId id, CancellationToken cancellationToken);

    Task<List<Recipe>> GetSubscribedRecipesAsync(int pageNumber, int pageSize, List<Subscription> subscriptions, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken);

    Task UpdateRecipeAsync(Recipe recipe, CancellationToken cancellationToken);

    Task<int> GetTotalCountAsync(Expression<Func<Recipe, bool>> predicate);

    Task<int> GetSubscriptionsCountAsync(Expression<Func<Recipe, bool>> predicate, List<Subscription> subscriptions, CancellationToken cancellationToken);

    Task<List<Recipe>> GetSavedRecipesAsync(int pageNumber, int pageSize, Expression<Func<Recipe, bool>> predicate, List<SavedRecipe> saves, CancellationToken cancellationToken);

    Task<int> GetSavedRecipesCountAsync(Expression<Func<Recipe, bool>> predicate, List<SavedRecipe> saves, CancellationToken cancellationToken);
}
