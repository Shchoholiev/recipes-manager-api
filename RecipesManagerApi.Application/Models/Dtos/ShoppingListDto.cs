namespace RecipesManagerApi.Application.Models.Dtos;

public class ShoppingListDto
{
	public string Id { get; set; }
	
	public string? Name { get; set; }
	
	public List<IngredientDto>? Ingredients { get; set; }
	
	public List<RecipeDto>? Recipes { get; set; }
	
	public string? Notes { get; set; }
	
	public List<ContactDto>? SentTo { get; set; }
}