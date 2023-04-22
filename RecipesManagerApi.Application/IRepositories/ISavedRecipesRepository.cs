using MongoDB.Bson;
using System.Linq.Expressions;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;

public interface ISavedRecipesRepository : IBaseRepository<SavedRecipe>
{
    Task<List<SavedRecipe>> GetUsersSavesAsync(ObjectId id, CancellationToken cancellationToken);

}

