namespace RecipesManagerApi.Application.Models.ShortDtos;

public class IngredientShortDto
{
    public string Name { get; set; }

    public string? Units { get; set; }

    public int? CaloriesPerUnit { get; set; }
}
