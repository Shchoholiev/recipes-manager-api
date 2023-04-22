using System;
using System.Linq.Expressions;
using RecipesManagerApi.Domain.Entities;
using MongoDB.Bson;

namespace RecipesManagerApi.Application.IRepositories;

public interface ISubscriptionsRepository : IBaseRepository<Subscription>
{
    Task<List<Subscription>> GetUsersSubscriptionsAsync(ObjectId id, CancellationToken cancellationToken);
}

