using HotChocolate.Authorization;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class RolesQuery
{
    [Authorize]
    public Task<PagedList<RoleDto>> GetRolesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
        [Service] IRolesService service)
        => service.GetRolesPageAsync(pageNumber, pageSize, cancellationToken);

    [Authorize]
    public Task<RoleDto> GetRoleAsync(string id, CancellationToken cancellationToken,
        [Service] IRolesService service)
        => service.GetRoleAsync(id, cancellationToken);
}
