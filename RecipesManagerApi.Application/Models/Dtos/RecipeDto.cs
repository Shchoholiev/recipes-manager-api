namespace RecipesManagerApi.Application.Models.Dtos;

public class RecipeDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string? Text { get; set; }

    public ImageDto? Thumbnail { get; set; }

    public List<IngredientDto>? Ingredients { get; set; }

    public string? IngredientsText { get; set; }

    public List<CategoryDto> Categories { get; set; }

    public int? Calories { get; set; }

    public int? ServingsCount { get; set; }

    public int? MinutesToCook { get; set; }

    public bool IsPublic { get; set; }

    public bool IsSaved { get; set; }

    public string CreatedById { get; set; }

    public UserDto CreatedBy { get; set; }

    public DateTime CreatedDateUtc { get; set; }
}
