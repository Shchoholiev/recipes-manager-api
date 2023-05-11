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

    public async Task UpdateSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken)
    {
        await this._collection.ReplaceOneAsync(Builders<Subscription>.Filter.Eq(x => x.Id, subscription.Id), subscription, new ReplaceOptions(), cancellationToken);
    }

    public async Task<List<Subscription>> GetUsersSubscriptionsAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await (await this._collection.FindAsync(x => x.CreatedById == id)).ToListAsync();
    }

}



