using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Infrastructure.Database;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Bson;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class SubscriptionsRepository : BaseRepository<Subscription>, ISubscriptionsRepository
{
	public SubscriptionsRepository(MongoDbContext db) : base(db, "Subscriptions"){ }

	public async Task<Subscription> GetSubscriptionAsync(ObjectId id, CancellationToken cancellationToken)
	{
		return await (await this._collection.FindAsync(x => x.Id == id && x.IsDeleted == false)).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<int> GetTotalCountAsync(Expression<Func<Subscription, bool>> predicate)
	{
		return (int)(await this._collection.CountDocumentsAsync<Subscription>(x => x.IsDeleted == false));
	}

	public async Task<Subscription> UpdateSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken)
	{
		var updateDefinition = Builders<Subscription>.Update
			.Set(s => s.IsAccessFull, subscription.IsAccessFull)
			.Set(s => s.LastModifiedById, subscription.LastModifiedById)
			.Set(s => s.LastModifiedDateUtc, subscription.LastModifiedDateUtc);
		
		var options = new FindOneAndUpdateOptions<Subscription>
		{
			ReturnDocument = ReturnDocument.After
		};
		
		return await this._collection.FindOneAndUpdateAsync(
			Builders<Subscription>.Filter.Eq(s => s.Id, subscription.Id), updateDefinition, options, cancellationToken);
			
	}

	public async Task<List<Subscription>> GetUsersSubscriptionsAsync(ObjectId id, CancellationToken cancellationToken)
	{
		return await (await this._collection.FindAsync(x => x.CreatedById == id)).ToListAsync();
	}

}



