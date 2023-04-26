using RecipesManagerApi.Domain.Entities;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IRepositories;

public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    Task<Subscription> GetSubscriptionAsync(ObjectId id, CancellationToken cancellationToken);

    Task UpdateSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken);

    Task<int> GetTotalCountAsync(Expression<Func<Subscription, bool>> predicate);
}

