using RecipesManagerApi.Application.Models.Dtos;

namespace RecipesManagerApi.Application.IServices;
public interface IUserActivityService
{ 
    Task<RecipeViewActivityDto> AddRecipeViewActivityAsync(RecipeViewActivityDto dto, CancellationToken cancellationToken);
}
