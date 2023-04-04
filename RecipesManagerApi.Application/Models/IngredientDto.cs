namespace RecipesManagerApi.Application.Models;

public class IngredientDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string? Units { get; set; }

    public Double? Amount { get; set; }
}
