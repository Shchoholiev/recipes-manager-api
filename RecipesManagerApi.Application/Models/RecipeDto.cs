using MongoDB.Bson;

namespace RecipesManagerApi.Application.Models;

public class RecipeDto
{
    public ObjectId Id { get; set; }

    public String Name { get; set; }

    public ImageDto? Thumbnail { get; set; }

    public List<IngredientDto>? Ingredients { get; set; }

    public String? IngredientsText {get; set;}

    public List<CategoryDto> Categories { get; set; }

    public int? Calories { get; set; }

    public int? ServingsCount { get; set; }
}
