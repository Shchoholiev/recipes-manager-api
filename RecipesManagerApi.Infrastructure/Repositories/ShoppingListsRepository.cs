using System.Linq.Expressions;
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
		return await this.GetShoppingListLookedUpAsync(shoppingList.Id, cancellationToken);
	}

	public async Task<ShoppingListLookedUp> GetShoppingListLookedUpAsync(ObjectId id, CancellationToken cancellationToken)
	{
		var lookupRecipes = new BsonDocument("$lookup",
			new BsonDocument
			{
				{ "from", "Recipes" },
				{ "localField", "RecipesIds" },
				{ "foreignField", "_id" },
				{ "as", "Recipes" }
			});
		var lookupContacts = new BsonDocument("$lookup",
			new BsonDocument
			{
				{ "from", "Contacts" },
				{ "localField", "SentTo" },
				{ "foreignField", "_id" },
				{ "as", "SentToContacts" }
			});
		var pipeline = new BsonDocument[]{
			lookupRecipes,
			lookupContacts,
			new BsonDocument("$match", new BsonDocument("_id", id)),
			new BsonDocument("$match", new BsonDocument("IsDeleted", false))
		};

		return await (await this._collection.AggregateAsync<ShoppingListLookedUp>(pipeline, new AggregateOptions(), cancellationToken))
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<ShoppingListLookedUp> UpdateShoppingListAsync(ShoppingList shoppingList, CancellationToken cancellationToken)
	{
		var oldEntity = await this.GetShoppingListAsync(shoppingList.Id, cancellationToken);
		shoppingList.CreatedById = oldEntity.CreatedById;
		shoppingList.CreatedDateUtc = oldEntity.CreatedDateUtc;
		if(oldEntity.SentTo != null && shoppingList.SentTo == null)
		{
			shoppingList.SentTo = oldEntity.SentTo;
		}
		await this._collection.ReplaceOneAsync(x => x.Id == shoppingList.Id, shoppingList, new ReplaceOptions(), cancellationToken);
		return await this.GetShoppingListLookedUpAsync(shoppingList.Id, cancellationToken);
	}
	
	public async Task<List<ShoppingListLookedUp>> GetPageAsync(int pageNumber, int pageSize, ObjectId userId, CancellationToken cancellationToken)
	{
		var lookupRecipes = new BsonDocument("$lookup",
			new BsonDocument
			{
				{ "from", "Recipes" },
				{ "localField", "RecipesIds" },
				{ "foreignField", "_id" },
				{ "as", "Recipes" }
			});
		var lookupContacts = new BsonDocument("$lookup",
			new BsonDocument
			{
				{ "from", "Contacts" },
				{ "localField", "SentTo" },
				{ "foreignField", "_id" },
				{ "as", "SentToContacts" }
			});	
		var pipeline = new BsonDocument[]{
			lookupRecipes,
			lookupContacts,
			new BsonDocument("$match", new BsonDocument("CreatedById", userId)),
			new BsonDocument("$match", new BsonDocument("IsDeleted", false)),
			new BsonDocument("$skip", (pageNumber - 1) * pageSize),
			new BsonDocument("$limit", pageSize)
		};
		
		return await (await this._collection.AggregateAsync<ShoppingListLookedUp>(pipeline, new AggregateOptions(), cancellationToken))
			.ToListAsync(cancellationToken);
	}

	public async Task<ShoppingList> GetShoppingListAsync(ObjectId id, CancellationToken cancellationToken)
	{
		return await (await this._collection.FindAsync(x=>x.Id == id && x.IsDeleted == false)).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<int> GetTotalCountAsync(Expression<Func<ShoppingList, bool>> predicate)
	{
		var filter = Builders<ShoppingList>.Filter.Where(predicate);
		return (int)(await this._collection.CountDocumentsAsync(filter));
	}
}