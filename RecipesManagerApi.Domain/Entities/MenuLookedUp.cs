namespace RecipesManagerApi.Domain.Entities;

public class MenuLookedUp : Menu
{
	public List<Recipe>? Recipes;
	
	public List<Contact>? SentToContacts;
}