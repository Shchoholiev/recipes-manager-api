using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Application.IServices;

public interface ISharedRecipesService
{
    Task<SharedRecipeDto> AddSharedRecipeAsync(SharedRecipeCreateDto dto, CancellationToken cancellationToken);

    Task<SharedRecipeDto> GetSharedRecipeAsync(string id, CancellationToken cancellationToken);

    Task<SharedRecipeDto> AccessSharedRecipeAsync(string id, CancellationToken cancellationToken);

    Task<OperationDetails> DeleteSharedRecipeAsync(string id, CancellationToken cancellationToken);
}
