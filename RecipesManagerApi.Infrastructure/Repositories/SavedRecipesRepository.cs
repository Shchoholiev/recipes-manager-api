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

	public async Task<List<SavedRecipe>> GetUsersSavesAsync(ObjectId id, CancellationToken cancellationToken)
	{
		return await(await this._collection.FindAsync<SavedRecipe>(x => x.CreatedById == id)).ToListAsync();
	}
   
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
	
	public async Task<SavedRecipe> DeleteSavedRecipeAsync(SavedRecipe recipe, CancellationToken cancellationToken)
	{
		var filter = Builders<SavedRecipe>.Filter
			.Eq(r => r.CreatedById, recipe.CreatedById) & Builders<SavedRecipe>.Filter
			.Eq(r => r.RecipeId, recipe.RecipeId) & Builders<SavedRecipe>.Filter
			.Eq(r => r.IsDeleted, false);
		var updateDefinition = Builders<SavedRecipe>.Update
			.Set(r => r.IsDeleted, true)
			.Set(r => r.LastModifiedById, recipe.LastModifiedById)
			.Set(r => r.LastModifiedDateUtc, recipe.LastModifiedDateUtc);
		var options = new FindOneAndUpdateOptions<SavedRecipe>
		{
			ReturnDocument = ReturnDocument.After
		};
		return await this._collection.FindOneAndUpdateAsync(filter, updateDefinition, options, cancellationToken);
	}
}

