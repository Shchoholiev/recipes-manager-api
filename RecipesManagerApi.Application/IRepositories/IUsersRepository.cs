﻿using MongoDB.Bson;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IRepositories;

public interface IUsersRepository : IBaseRepository<User>
{
    Task<User> GetUserAsync(ObjectId id, CancellationToken cancellationToken);

    Task<User> GetUserAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken);

    Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken);
}
