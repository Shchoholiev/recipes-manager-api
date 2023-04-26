using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.IRepositories;
using MongoDB.Bson;
using System.Linq.Expressions;
using RecipesManagerApi.Infrastructure.Database;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
{
    public SubscriptionRepository(MongoDbContext db) : base(db, "Subscriptions") { }

    public async Task<Subscription> GetSubscriptionAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await(await this._collection.FindAsync(x => x).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<int> GetTotalCountAsync(Expression<Func<Subscription, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<Subscription> UpdateSubscriptionAsync(ObjectId id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

