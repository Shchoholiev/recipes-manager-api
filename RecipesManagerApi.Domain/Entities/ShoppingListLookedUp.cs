namespace RecipesManagerApi.Domain.Entities;

public class ShoppingListLookedUp : ShoppingList
{
	public List<Recipe>? Recipes { get; set; }
}