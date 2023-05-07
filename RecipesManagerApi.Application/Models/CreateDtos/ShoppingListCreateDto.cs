using HotChocolate;
using RecipesManagerApi.Application.Models.Dtos;

namespace RecipesManagerApi.Application.Models.CreateDtos;

[GraphQLName("ShoppingListInput")]
public class ShoppingListCreateDto
{
	public string? Id { get; set; }
	
	public string? Name { get; set; }
	
	public List<IngredientDto>? Ingredients { get; set; } 
	
	public List<string>? RecipesIds { get; set; } 
	
	public string? Notes { get; set; } 
}