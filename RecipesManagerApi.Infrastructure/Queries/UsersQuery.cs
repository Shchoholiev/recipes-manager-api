using HotChocolate.Authorization;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class UsersQuery
{
    [Authorize]
    public Task<UserDto> GetUserAsync(string id, CancellationToken cancellationToken,
    [Service] IUsersService usersService)
    => usersService.GetUserAsync(id, cancellationToken);

    [Authorize]
    public Task<PagedList<UserDto>> GetUsersPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
    [Service] IUsersService usersService)
    => usersService.GetUsersPageAsync(pageNumber, pageSize, cancellationToken);
}
