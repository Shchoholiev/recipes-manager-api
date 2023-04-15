namespace RecipesManagerApi.Application.Models;

public class SharedRecipeDto
{
    public string Id { get; set; }

    public string RecipeId { get; set; }

    public int VisitsCount { get; set; }
}
