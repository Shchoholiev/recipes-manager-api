using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.Models.CreateDtos;
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

	public async Task<ShoppingListLookedUp> UpdateShoppingListAsync(ObjectId id, ShoppingList shoppingList, CancellationToken cancellationToken)
	{
		var updateDefinition = Builders<ShoppingList>.Update
			.Set(l => l.Name, shoppingList.Name)
			.Set(l => l.Ingredients, shoppingList.Ingredients)
			.Set(l => l.RecipesIds, shoppingList.RecipesIds)
			.Set(l => l.Notes, shoppingList.Notes)
			.Set(l => l.LastModifiedById, shoppingList.LastModifiedById)
			.Set(l => l.LastModifiedDateUtc, shoppingList.LastModifiedDateUtc);
			
		await this._collection.FindOneAndUpdateAsync(
				Builders<ShoppingList>.Filter.Eq(l => l.Id, id), updateDefinition, new FindOneAndUpdateOptions<ShoppingList>(), cancellationToken);
		return await this.GetShoppingListLookedUpAsync(id, cancellationToken);
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

	public async Task UpdateShoppingListSentToAsync(ShoppingList shoppingList, CancellationToken cancellationToken)
	{
		var updateDefinition = Builders<ShoppingList>.Update
			.Set(l => l.SentTo, shoppingList.SentTo)
			.Set(l => l.LastModifiedById, shoppingList.LastModifiedById)
			.Set(l => l.LastModifiedDateUtc, shoppingList.LastModifiedDateUtc);
		
		await this._collection.FindOneAndUpdateAsync(
			Builders<ShoppingList>.Filter.Eq(l => l.Id, shoppingList.Id), updateDefinition, new FindOneAndUpdateOptions<ShoppingList>(), cancellationToken);
	}
}