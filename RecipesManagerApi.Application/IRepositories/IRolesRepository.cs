using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;
public interface IRolesRepository : IBaseRepository<Role>
{
    Task<Role> GetRoleAsync(ObjectId id, CancellationToken cancellationToken);
}
