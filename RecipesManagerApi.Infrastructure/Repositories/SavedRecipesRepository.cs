using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Infrastructure.Database;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Bson;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class SavedRecipesRepository : BaseRepository<SavedRecipe>, ISavedRecipesRepository
{
    public SavedRecipesRepository(MongoDbContext db) : base(db, "SavedRecipes") { }

    public async Task<List<SavedRecipe>> GetUsersSavesAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await(await this._collection.FindAsync<SavedRecipe>(x => x.UserId == id)).ToListAsync();
    }
   
}

