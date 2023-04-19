using MongoDB.Bson;

namespace RecipesManagerApi.Application.Models;

public class MenuCreateDto
{
	public string Name { get; set; }
	
	public List<ObjectId>? RecipesIds { get; set; }
	
	public string? Notes { get; set; }
	
	public List<ContactDto>? SentTo { get; set; }
	
	public DateTime? ForDateUtc { get; set; }
}