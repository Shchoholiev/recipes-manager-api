﻿using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IRepositories;
public interface IRolesRepository : IBaseRepository<Role>
{
    Task<Role> GetRoleAsync(ObjectId id, CancellationToken cancellationToken);

    Task<Role> GetRoleAsync(Expression<Func<Role, bool>> predicate, CancellationToken cancellationToken);
}
