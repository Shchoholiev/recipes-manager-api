using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Database;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class ShoppingListsRepository : BaseRepository<ShoppingList>, IShoppingListsRepository
{
	public ShoppingListsRepository(MongoDbContext db) : base(db, "ShoppingLists") {}

	public async Task<ShoppingListLookedUp> AddShoppingListAsync(ShoppingList shoppingList, CancellationToken cancellationToken)
	{
		await this._collection.InsertOneAsync(shoppingList, new InsertOneOptions(), cancellationToken);
		var lookup = new BsonDocument("$lookup",
			new BsonDocument
			{
				{ "from", "Recipes" },
				{ "localField", "RecipesIds" },
				{ "foreignField", "_id" },
				{ "as", "Recipes" }
			});
			
		var pipeline = new BsonDocument[]{
			lookup,
			new BsonDocument("$match", new BsonDocument("_id", shoppingList.Id)),
			new BsonDocument("$match", new BsonDocument("IsDeleted", false))
		};
		
		return await (await this._collection.AggregateAsync<ShoppingListLookedUp>(pipeline, new AggregateOptions(), cancellationToken))
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<ShoppingListLookedUp> GetShoppingListAsync(ObjectId id, CancellationToken cancellationToken)
	{
		var lookup = new BsonDocument("$lookup",
			new BsonDocument
			{
				{ "from", "Recipes" },
				{ "localField", "RecipesIds" },
				{ "foreignField", "_id" },
				{ "as", "Recipes" }
			});

		var pipeline = new BsonDocument[]{
			lookup,
			new BsonDocument("$match", new BsonDocument("_id", id)),
			new BsonDocument("$match", new BsonDocument("IsDeleted", false))
		};

		return await (await this._collection.AggregateAsync<ShoppingListLookedUp>(pipeline, new AggregateOptions(), cancellationToken))
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task UpdateShoppingListAsync(ShoppingList shoppingList, CancellationToken cancellationToken)
	{
		await this._collection.ReplaceOneAsync(x => x.Id == shoppingList.Id, shoppingList, new ReplaceOptions(), cancellationToken);
	}
}