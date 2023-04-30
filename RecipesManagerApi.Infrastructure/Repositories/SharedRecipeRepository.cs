using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Database;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class SharedRecipeRepository : BaseRepository<SharedRecipe>, ISharedRecipesRepository
{
    public SharedRecipeRepository(MongoDbContext db) : base(db, "SharedRecipes"){}

    public async Task<SharedRecipe> GetSharedRecipeAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await(await this._collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateSharedRecipeAsync(SharedRecipe recipe, CancellationToken cancellationToken)
    {
        await this._collection.ReplaceOneAsync(x => x.Id == recipe.Id, recipe, new ReplaceOptions(), cancellationToken);
    }
}
