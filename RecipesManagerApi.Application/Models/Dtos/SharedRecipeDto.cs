namespace RecipesManagerApi.Application.Models.Dtos;

public class SharedRecipeDto
{
    public string Id { get; set; }

    public string RecipeId { get; set; }

    public int VisitsCount { get; set; }
}
