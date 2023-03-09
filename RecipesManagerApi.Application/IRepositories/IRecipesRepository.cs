using MongoDB.Bson;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IRepositories;

public interface IRecipesRepository : IBaseRepository<Recipe>
{
    Task<List<Recipe>> GetRecipesPageAsync(PageParameters pageParameters, CancellationToken cancellationToken);

    Task<List<Recipe>> GetRecipesPageAsync(PageParameters pageParameters, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken);

    Task<Recipe> GetRecipeAsync(ObjectId id, CancellationToken cancellationToken);

    Task UpdateAsync(Recipe recipe, CancellationToken cancellationToken);
}
