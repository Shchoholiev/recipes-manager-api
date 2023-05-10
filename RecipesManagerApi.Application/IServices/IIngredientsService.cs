using RecipesManagerApi.Application.Models.Dtos;

namespace RecipesManagerApi.Application.IServices;

public interface IIngredientsService
{
    IAsyncEnumerable<IngredientDto> ParseIngredientsAsync(string text, CancellationToken cancellationToken);

    IAsyncEnumerable<IngredientDto> EstimateIngredientsCaloriesAsync(List<IngredientDto> ingredients, CancellationToken cancellationToken);
}
