using HotChocolate;

namespace RecipesManagerApi.Application.Models.CreateDtos;

[GraphQLName("MenuInput")]
public class MenuCreateDto
{
	public string Name { get; set; }
	
	public List<string>? RecipesIds { get; set; }
	
	public string? Notes { get; set; }
	
	public DateTime? ForDateUtc { get; set; }
}