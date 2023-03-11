using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface IRolesService
{
    Task AddRoleAsync(RoleDto dto, CancellationToken cancellationToken);

    Task<PagedList<RoleDto>> GetRolesPageAsync(PageParameters pageParameters, CancellationToken cancellationToken);

    Task<RoleDto> GetRoleAsync(ObjectId id, CancellationToken cancellationToken);
}
