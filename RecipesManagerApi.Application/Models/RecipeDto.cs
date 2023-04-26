using RecipesManagerApi.Application.Models.Dtos;

namespace RecipesManagerApi.Application.Models;

public class RecipeDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public ImageDto? Thumbnail { get; set; }

    public List<IngredientDto>? Ingredients { get; set; }

    public string? IngredientsText {get; set;}

    public List<CategoryDto> Categories { get; set; }

    public int? Calories { get; set; }

    public int? ServingsCount { get; set; }
}