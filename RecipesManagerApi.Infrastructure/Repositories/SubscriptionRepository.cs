using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.IRepositories;
using MongoDB.Bson;
using System.Linq.Expressions;
using RecipesManagerApi.Infrastructure.Database;
using MongoDB.Driver;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
{
    public SubscriptionRepository(MongoDbContext db) : base(db, "Subscriptions") { }

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
}

