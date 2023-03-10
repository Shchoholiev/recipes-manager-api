using MongoDB.Bson;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IRepositories;
public interface IRolesRepository : IBaseRepository<Role>
{
    Task<List<Role>> GetRolesPageAsync(PageParameters pageParameters, CancellationToken cancellationToken);

    Task<List<Role>> GetRolesPageAsync(PageParameters pageParameters, Expression<Func<Role, bool>> predicate, CancellationToken cancellationToken);

    Task<Role> GetRoleAsync(ObjectId id, CancellationToken cancellationToken);
}
