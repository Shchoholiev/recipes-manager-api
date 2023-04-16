using RecipesManagerApi.Application.Models;

namespace RecipesManagerApi.Application.IServices;

public interface IIngredientsService
{
    IAsyncEnumerable<IngredientDto> ParseIngredientsAsync(string text, CancellationToken cancellationToken);
}
