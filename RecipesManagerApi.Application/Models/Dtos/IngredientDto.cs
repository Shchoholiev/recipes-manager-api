namespace RecipesManagerApi.Application.Models.Dtos;

public class IngredientDto
{
    public string? Id { get; set; }

    public string Name { get; set; }

    public string? Units { get; set; }

    public Double? Amount { get; set; }

    public int? CaloriesPerUnit { get; set; }

    public int? TotalCalories { get; set; }
}
