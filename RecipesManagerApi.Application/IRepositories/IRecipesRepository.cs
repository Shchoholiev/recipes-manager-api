using MongoDB.Bson;
using RecipesManagerApi.Application.Models.LookUps;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IRepositories;

public interface IRecipesRepository : IBaseRepository<Recipe>
{
    Task<Recipe> UpdateRecipeAsync(ObjectId id, Recipe recipe, CancellationToken cancellationToken);

    Task<Recipe> UpdateRecipeThumbnailAsync(ObjectId id, Recipe recipe, CancellationToken cancellationToken);

    Task<Recipe> GetRecipeAsync(ObjectId id, CancellationToken cancellationToken);

    Task<RecipeLookUp> GetRecipeAsync(ObjectId id, ObjectId userId, CancellationToken cancellationToken);

    Task<List<RecipeLookUp>> GetRecipesPageAsync(int pageNumber, int pageSize, ObjectId userId, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken);

    Task<int> GetTotalCountAsync(Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken);

    Task<List<RecipeLookUp>> GetSubscribedRecipesAsync(int pageNumber, int pageSize, ObjectId userId, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken);

    Task<int> GetSubscriptionsCountAsync(Expression<Func<Recipe, bool>> predicate, ObjectId userId, CancellationToken cancellationToken);

    Task<List<RecipeLookUp>> GetSavedRecipesAsync(int pageNumber, int pageSize, ObjectId userId, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken);

    Task<int> GetSavedRecipesCountAsync(Expression<Func<Recipe, bool>> predicate, ObjectId userId, CancellationToken cancellationToken);
}
