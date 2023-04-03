using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IRepositories;

public interface IRecipesRepository : IBaseRepository<Recipe>
{
    Task<Recipe> GetRecipeAsync(ObjectId id, CancellationToken cancellationToken);

    Task UpdateRecipeAsync(Recipe recipe, CancellationToken cancellationToken);

    Task<int> GetTotalCountAsync(Expression<Func<Recipe, bool>> predicate);
}
