using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Database;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class MenusRepository : BaseRepository<Menu>, IMenusRepository
{
	public MenusRepository(MongoDbContext db) : base(db, "Menus") { }
	public async Task<MenuLookedUp> GetMenuAsync(ObjectId id, CancellationToken cancellationToken)
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

		return await (await this._collection.AggregateAsync<MenuLookedUp>(pipeline, new AggregateOptions(), cancellationToken))
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<int> GetTotalCountAsync(Expression<Func<Menu, bool>> predicate)
	{
		var filter = Builders<Menu>.Filter.Where(predicate);
		return (int)(await this._collection.CountDocumentsAsync(filter));
	}

	public async Task<MenuLookedUp> UpdateMenuAsync(Menu menu, CancellationToken cancellationToken)
	{
		await this._collection.ReplaceOneAsync(x => x.Id == menu.Id, menu, new ReplaceOptions(), cancellationToken);
		
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
			new BsonDocument("$match", new BsonDocument("_id", menu.Id)),
			new BsonDocument("$match", new BsonDocument("IsDeleted", false))
		};
			
		return  await (await this._collection.AggregateAsync<MenuLookedUp>(pipeline, new AggregateOptions(), cancellationToken))
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<List<MenuLookedUp>> GetPageAsync(int pageNumber, int pageSize, ObjectId userId, CancellationToken cancellationToken)
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
			new BsonDocument("$match", new BsonDocument("_id", userId)),
			new BsonDocument("$match", new BsonDocument("IsDeleted", false)),
			new BsonDocument("$skip", (pageNumber - 1) * pageSize),
			new BsonDocument("$limit", pageSize)
		};
		
		return await (await this._collection.AggregateAsync<MenuLookedUp>(pipeline, new AggregateOptions(), cancellationToken))
			.ToListAsync(cancellationToken);
	}
	
	public async Task<MenuLookedUp> AddMenuAsync(Menu entity, CancellationToken cancellationToken)
	{
		await this._collection.InsertOneAsync(entity, new InsertOneOptions() ,cancellationToken);
		
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
			new BsonDocument("$match", new BsonDocument("_id", entity.Id))
		};
			
		return  await (await this._collection.AggregateAsync<MenuLookedUp>(pipeline, new AggregateOptions(), cancellationToken))
					.FirstOrDefaultAsync(cancellationToken);
	}
}
