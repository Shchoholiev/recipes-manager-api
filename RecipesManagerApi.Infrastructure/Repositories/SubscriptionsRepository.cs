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

    public async Task<List<Subscription>> GetUsersSubscriptionsAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await (await this._collection.FindAsync(x => x.CreatedById == id)).ToListAsync();
    }

}



