namespace RecipesManagerApi.Application.Models.Dtos;

public class MenuDto
{
	public string Id { get; set; }
	
	public string Name { get; set; }
	
	public List<RecipeDto>? Recipes { get; set; }
	
	public string? Notes { get; set; }
	
	public List<ContactDto>? SentTo { get; set; }
	
	public DateTime? ForDateUtc { get; set; }
}