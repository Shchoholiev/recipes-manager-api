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
		return await(await this._collection.FindAsync(x => x.Id == id && x.IsDeleted == false)).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<SharedRecipe> UpdateSharedRecipeAsync(ObjectId id, SharedRecipe recipe, CancellationToken cancellationToken)
	{
		var updateDefinition = Builders<SharedRecipe>.Update
			.Set(r => r.RecipeId, recipe.RecipeId)
			.Set(r => r.LastModifiedById, recipe.LastModifiedById)
			.Set(r => r.LastModifiedDateUtc, recipe.LastModifiedDateUtc);
			
		var options = new FindOneAndUpdateOptions<SharedRecipe>
		{
			ReturnDocument = ReturnDocument.After
		};
		
		return await this._collection.FindOneAndUpdateAsync(
			Builders<SharedRecipe>.Filter.Eq(r => r.Id, id), updateDefinition, options, cancellationToken);	
	}

    public async Task<SharedRecipe> UpdateSharedRecipeVisitsAsync(ObjectId id, SharedRecipe recipe, CancellationToken cancellationToken)
    {
        var updateDefinition = Builders<SharedRecipe>.Update
			.Inc(r => r.VisitsCount, 1)
			.Set(r => r.LastModifiedById, recipe.LastModifiedById)
			.Set(r => r.LastModifiedDateUtc, recipe.LastModifiedDateUtc);
			
		var options = new FindOneAndUpdateOptions<SharedRecipe>
		{
			ReturnDocument = ReturnDocument.After
		};
		
		return await this._collection.FindOneAndUpdateAsync(
			Builders<SharedRecipe>.Filter.Eq(r => r.Id, id), updateDefinition, options, cancellationToken);	
    }
}
