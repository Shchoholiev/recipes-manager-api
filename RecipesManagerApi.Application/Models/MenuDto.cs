namespace RecipesManagerApi.Application.Models;

public class MenuDto
{
	public string Id { get; set; }
	
	public string Name { get; set; }
	
	public List<RecipeDto>? Recipes { get; set; }
	
	public string? Notes { get; set; }
	
	public List<ContactDto>? SentTo { get; set; }
	
	public DateTime? ForDateUtc { get; set; }
}