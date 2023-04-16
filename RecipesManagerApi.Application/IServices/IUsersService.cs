using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;
public interface IUsersService
{
    Task AddUserAsync(UserDto dto, CancellationToken cancellationToken);

    Task<PagedList<UserDto>> GetUsersPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<UserDto> GetUserAsync(string id, CancellationToken cancellationToken);

    Task UpdateUserAsync (UserDto dto, CancellationToken cancellationToken);
}
