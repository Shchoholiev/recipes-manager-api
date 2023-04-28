using System;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Operations;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface ISavedRecipesService
{
    Task<SavedRecipeDto> AddSavedRecipeAsync(SavedRecipeCreateDto dto, CancellationToken cancellationToken);

    Task<SavedRecipeDto> GetSavedRecipeAsync(string id, CancellationToken cancellationToken);

    Task<PagedList<SavedRecipeDto>> GetSavedRecipesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<OperationDetails> DeleteSavedRecipeAsync(SavedRecipeDto dto, CancellationToken cancellationToken);
}

