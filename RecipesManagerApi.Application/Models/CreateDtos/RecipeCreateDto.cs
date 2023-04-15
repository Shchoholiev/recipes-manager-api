using Microsoft.AspNetCore.Http;

namespace RecipesManagerApi.Application.Models;

public class RecipeCreateDto
{
    public string Name { get; set; }

    public IFormFile? Thumbnail { get; set; }

    public List<IngredientDto>? Ingredients { get; set; }

    public string? IngredientsText {get; set;}

    public List<CategoryDto> Categories { get; set; }

    public int? Calories { get; set; }

    public int? ServingsCount { get; set; }
}
