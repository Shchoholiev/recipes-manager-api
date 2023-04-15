using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface ISharedRecipesService
{
    Task AddSharedRecipeAsync(SharedRecipeDto dto, CancellationToken cancellationToken);

    Task UpdateSharedRecipeAsync(SharedRecipeDto dto, CancellationToken cancellationToken);

    Task<SharedRecipeDto> GetSharedRecipeAsync(string id, CancellationToken cancellationToken);

    Task<PagedList<SharedRecipeDto>> GetSharedRecipesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
}
