using MongoDB.Bson;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface IRolesService
{
    Task<RoleDto> AddRoleAsync(RoleCreateDto dto, CancellationToken cancellationToken);

    Task<PagedList<RoleDto>> GetRolesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<RoleDto> GetRoleAsync(string id, CancellationToken cancellationToken);
}
