using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IRepositories;

public interface IRecipesRepository : IBaseRepository<Recipe>
{
    Task<Recipe> GetRecipeAsync(ObjectId id, CancellationToken cancellationToken);

    Task<List<Recipe>> GetSubscribedRecipesAsync(int pageNumber, int pageSize, ObjectId id, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken);

    Task UpdateRecipeAsync(Recipe recipe, CancellationToken cancellationToken);

    Task<int> GetTotalCountAsync(Expression<Func<Recipe, bool>> predicate);

    Task<int> GetSubscriptionsCountAsync(ObjectId userId, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken);

    Task<List<Recipe>> GetSavedRecipesAsync(int pageNumber, int pageSize, ObjectId userId, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken);

    Task<int> GetSavedRecipesCountAsync(ObjectId userId, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken);
}
