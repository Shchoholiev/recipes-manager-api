using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.IRepositories;
using MongoDB.Bson;
using System.Linq.Expressions;
using RecipesManagerApi.Infrastructure.Database;
using MongoDB.Driver;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class SavedRecipesRepository : BaseRepository<SavedRecipe>, ISavedRecipesRepository
{
    public SavedRecipesRepository(MongoDbContext db) : base(db, "SavedRecipes") { }

    public async Task<SavedRecipe> GetSavedRecipeAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await (await this._collection.FindAsync(x => x.Id == id && x.IsDeleted == false)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(Expression<Func<SavedRecipe, bool>> predicate)
    {
        return (int)(await this._collection.CountDocumentsAsync<SavedRecipe>(x => x.IsDeleted == false));
    }

    public async Task UpdateSavedRecipeAsync(SavedRecipe recipe, CancellationToken cancellationToken)
    {
        await this._collection.ReplaceOneAsync(Builders<SavedRecipe>.Filter.Eq(x => x.Id, recipe.Id), recipe, new ReplaceOptions(), cancellationToken);
    }
}

