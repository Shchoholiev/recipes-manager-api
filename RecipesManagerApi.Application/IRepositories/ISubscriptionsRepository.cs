using System;
using System.Linq.Expressions;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;

public interface ISubscriptionsRepository : IBaseRepository<Subscription>
{
    Task<int> GetTotalCountAsync(Expression<Func<Subscription, bool>> predicate);
}

