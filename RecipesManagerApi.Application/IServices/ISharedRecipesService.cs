using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Operations;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface ISharedRecipesService
{
    Task<SharedRecipeDto> AddSharedRecipeAsync(SharedRecipeCreateDto dto, CancellationToken cancellationToken);

    Task<OperationDetails> UpdateSharedRecipeAsync(SharedRecipeDto dto, CancellationToken cancellationToken);

    Task<SharedRecipeDto> GetSharedRecipeAsync(string id, CancellationToken cancellationToken);

    Task<SharedRecipeDto> AccessSharedRecipeAsync(string id, CancellationToken cancellationToken);
}
