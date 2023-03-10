using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IServices;

public interface IRecipesService
{
    Task AddRecipeAsync(RecipeDto dto, CancellationToken cancellationToken);

    Task UpdateRecipeAsync(RecipeDto dto, CancellationToken cancellationToken);

    Task<RecipeDto> GetRecipeAsync(ObjectId id, CancellationToken cancellationToken);

    Task<PagedList<RecipeDto>> GetPageRecipesAsync(PageParameters pageParameters, CancellationToken cancellationToken);

}
