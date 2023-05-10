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
	
	public async Task<Menu> GetMenuAsync(ObjectId id, CancellationToken cancellationToken)
	{
		return await (await this._collection.FindAsync(x=>x.Id == id && x.IsDeleted == false)).FirstOrDefaultAsync(cancellationToken);
	}
	
	public async Task<MenuLookedUp> GetMenuLookedUpAsync(ObjectId id, CancellationToken cancellationToken)
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
		var oldEntity = await this.GetMenuAsync(menu.Id, cancellationToken);
		
		menu.CreatedById = oldEntity.CreatedById;
		menu.CreatedDateUtc = oldEntity.CreatedDateUtc;
		
		if(oldEntity.SentTo != null && menu.SentTo == null)
		{
			menu.SentTo = oldEntity.SentTo;
		}
		
		await this._collection.ReplaceOneAsync(x => x.Id == menu.Id, menu, new ReplaceOptions(), cancellationToken);
		return await this.GetMenuLookedUpAsync(menu.Id, cancellationToken);
	}

	public async Task<List<MenuLookedUp>> GetPageAsync(int pageNumber, int pageSize, ObjectId userId, CancellationToken cancellationToken)
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
		
		return await (await this._collection.AggregateAsync<MenuLookedUp>(pipeline, new AggregateOptions(), cancellationToken))
			.ToListAsync(cancellationToken);
	}
	
	public async Task<MenuLookedUp> AddMenuAsync(Menu entity, CancellationToken cancellationToken)
	{
		await this._collection.InsertOneAsync(entity, new InsertOneOptions() ,cancellationToken);
		return await this.GetMenuLookedUpAsync(entity.Id, cancellationToken);
	}
}
