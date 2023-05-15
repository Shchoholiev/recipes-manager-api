using System;
using System.Linq.Expressions;
using RecipesManagerApi.Domain.Entities;
using MongoDB.Bson;

namespace RecipesManagerApi.Application.IRepositories;

public interface ISubscriptionsRepository : IBaseRepository<Subscription>
{
    Task<Subscription> GetSubscriptionAsync(ObjectId id, CancellationToken cancellationToken);

    Task UpdateSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken);

    Task<int> GetTotalCountAsync(Expression<Func<Subscription, bool>> predicate);

    Task<List<Subscription>> GetUsersSubscriptionsAsync(ObjectId id, CancellationToken cancellationToken);
}

